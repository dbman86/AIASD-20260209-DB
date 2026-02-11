#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Run PostHubAPI test suites with various options

.DESCRIPTION
    Execute tests with filtering, coverage generation, and watch mode support

.PARAMETER Filter
    Test filter expression (e.g., "Category=Unit", "Priority=Critical")

.PARAMETER Coverage
    Generate code coverage report and open in browser

.PARAMETER Watch
    Run tests in watch mode for continuous feedback

.PARAMETER Verbose
    Enable verbose test output

.EXAMPLE
    .\run-tests.ps1
    Run all tests

.EXAMPLE
    .\run-tests.ps1 -Filter "Category=Unit"
    Run only unit tests

.EXAMPLE
    .\run-tests.ps1 -Coverage
    Run all tests with coverage report

.EXAMPLE
    .\run-tests.ps1 -Filter "Category=Security" -Verbose
    Run security tests with verbose output
#>

param(
    [string]$Filter = "",
    [switch]$Coverage = $false,
    [switch]$Watch = $false,
    [switch]$Verbose = $false
)

$ErrorActionPreference = "Stop"

Write-Host "🧪 PostHubAPI Test Runner" -ForegroundColor Cyan
Write-Host ""

# Build the dotnet test command
$testCommand = "dotnet test"

if ($Watch) {
    Write-Host "👀 Running tests in watch mode..." -ForegroundColor Yellow
    Write-Host "   Press Ctrl+C to exit" -ForegroundColor Gray
    Write-Host ""
    
    if ($Filter) {
        dotnet watch test --filter $Filter
    } else {
        dotnet watch test
    }
    exit
}

# Build test arguments
$testArgs = @()

if ($Filter) {
    $testArgs += "--filter"
    $testArgs += $Filter
    Write-Host "🔍 Filter: $Filter" -ForegroundColor Yellow
}

if ($Verbose) {
    $testArgs += "--verbosity"
    $testArgs += "detailed"
}

if ($Coverage) {
    Write-Host "📊 Generating code coverage report..." -ForegroundColor Yellow
    Write-Host ""
    
    $testArgs += "/p:CollectCoverage=true"
    $testArgs += "/p:CoverletOutputFormat=html"
    $testArgs += "/p:CoverletOutput=./TestResults/coverage/"
    $testArgs += "/p:ExcludeByFile=**/Program.cs"
    
    # Run tests with coverage
    & dotnet test @testArgs
    
    if ($LASTEXITCODE -eq 0) {
        $reportPath = Join-Path $PSScriptRoot "..\PostHubAPI.Tests\TestResults\coverage\index.html"
        
        if (Test-Path $reportPath) {
            Write-Host ""
            Write-Host "✅ Coverage report generated!" -ForegroundColor Green
            Write-Host "📂 Location: $reportPath" -ForegroundColor Cyan
            Write-Host ""
            Write-Host "Opening in browser..." -ForegroundColor Gray
            Start-Process $reportPath
        } else {
            Write-Host ""
            Write-Warning "Coverage report not found at expected location"
        }
    } else {
        Write-Host ""
        Write-Host "❌ Tests failed!" -ForegroundColor Red
        exit $LASTEXITCODE
    }
} else {
    # Run tests without coverage
    Write-Host "Running tests..." -ForegroundColor Yellow
    Write-Host ""
    
    & dotnet test @testArgs
    
    if ($LASTEXITCODE -eq 0) {
        Write-Host ""
        Write-Host "✅ All tests passed!" -ForegroundColor Green
    } else {
        Write-Host ""
        Write-Host "❌ Some tests failed!" -ForegroundColor Red
        exit $LASTEXITCODE
    }
}

Write-Host ""
Write-Host "💡 Quick commands:" -ForegroundColor Cyan
Write-Host "   Unit only:        .\run-tests.ps1 -Filter 'Category=Unit'" -ForegroundColor Gray
Write-Host "   Integration only: .\run-tests.ps1 -Filter 'Category=Integration'" -ForegroundColor Gray
Write-Host "   Security only:    .\run-tests.ps1 -Filter 'Category=Security'" -ForegroundColor Gray
Write-Host "   With coverage:    .\run-tests.ps1 -Coverage" -ForegroundColor Gray
Write-Host "   Watch mode:       .\run-tests.ps1 -Watch" -ForegroundColor Gray
