---
ai_generated: true
model: "anthropic/claude-3.5-sonnet@2024-10-22"
operator: "johnmillerATcodemag-com"
chat_id: "evergreen-dev-instructions-20260210"
prompt: |
  Generate comprehensive evergreen software development instruction file covering:
  - Continuous dependency management and updates
  - Code modernization and migration strategies
  - Technical debt prevention and management
  - Security vulnerability management and SBOM
  - Quality standards and automated enforcement
  - Infrastructure and tooling evolution
  - CI/CD integration and automation
  - Compliance gates and PR requirements
started: "2026-02-10T00:00:00Z"
ended: "2026-02-10T00:30:00Z"
task_durations:
  - task: "requirements analysis"
    duration: "00:05:00"
  - task: "content generation"
    duration: "00:20:00"
  - task: "examples and templates"
    duration: "00:05:00"
total_duration: "00:30:00"
ai_log: "ai-logs/2026/02/10/evergreen-dev-instructions-20260210/conversation.md"
source: ".github/prompts/create-evergreen-development-instructions.prompt.md"
applyTo: "**"
---

# Evergreen Software Development Instructions

## Overview

**Evergreen software development** is the practice of continuously maintaining, updating, and modernizing software to prevent technical debt accumulation and ensure long-term sustainability, security, and relevance. Like evergreen trees that remain vibrant year-round, evergreen software stays current, secure, and maintainable through proactive maintenance rather than reactive crisis management.

### Definition

Evergreen software development encompasses:

- **Continuous dependency updates** to maintain security and compatibility
- **Proactive code modernization** leveraging latest language and framework features
- **Technical debt prevention** through regular refactoring and quality standards
- **Automated security management** with vulnerability scanning and patching
- **Infrastructure evolution** keeping build systems and deployment pipelines current

### Business Value

- **Reduced Risk**: Minimizes security vulnerabilities and compliance violations
- **Sustainable Velocity**: Maintains consistent development speed over time
- **Lower Maintenance Costs**: Prevents expensive emergency fixes and rewrites
- **Talent Attraction**: Modern tech stacks attract and retain skilled developers
- **Competitive Advantage**: Faster feature delivery and market responsiveness

### Relationship to Technical Debt

Technical debt accumulates when shortcuts are taken or maintenance is deferred. Evergreen practices prevent debt by:

- Addressing small issues continuously rather than allowing accumulation
- Keeping dependencies current before breaking changes compound
- Refactoring incrementally rather than requiring major rewrites
- Maintaining test coverage to enable confident changes

### Scope and Applicability

These instructions apply to:

- All code repositories in active development or production use
- Libraries, applications, services, and infrastructure code
- Documentation, build systems, and deployment pipelines
- Both new development and legacy system maintenance

## Core Principles

### 1. Continuous Improvement Mindset

**Philosophy**: Software maintenance is not a phase—it's continuous.

**Practices**:

- Allocate 15-25% of sprint capacity to evergreen activities
- Treat dependency updates as routine work, not special projects
- Celebrate maintenance work equally with new features
- Build "evergreen time" into project schedules

### 2. Proactive vs Reactive Maintenance

**Proactive Indicators**:

- ✅ Regular scheduled dependency updates
- ✅ Security patches applied within SLA
- ✅ Code quality metrics show improvement trends
- ✅ Technical debt backlog decreases over time

**Reactive Red Flags**:

- ❌ Emergency security fixes dominate sprints
- ❌ Blocked by outdated dependencies for new features
- ❌ Major version migrations require months of work
- ❌ Build system failures halt all development

**Target**: 80% proactive, 20% reactive maintenance effort.

### 3. Sustainable Development Velocity

**Velocity Sustainability Metrics**:

- Story point completion remains consistent across quarters
- Build times do not increase by more than 10% annually
- PR review times remain under configured thresholds
- Developer survey scores for "ease of making changes" trend positive

**Threats to Velocity**:

- Accumulated technical debt slowing feature development
- Outdated tooling requiring manual workarounds
- Missing or outdated documentation
- Test suite brittleness or excessive run times

### 4. Balance Between Innovation and Stability

**Innovation Goals**:

- Adopt proven technologies within 6-12 months of GA release
- Evaluate emerging patterns through proof-of-concepts
- Budget 10-15% capacity for experimentation

**Stability Requirements**:

- Maintain backward compatibility for public APIs
- Use feature flags for risky changes
- Require comprehensive testing before major updates
- Maintain rollback capabilities for critical systems

## Dependency Management

### Update Frequency

| Dependency Type             | Update Cadence     | Approval Required        |
| --------------------------- | ------------------ | ------------------------ |
| Security patches (critical) | Within 24 hours    | Auto-merge if tests pass |
| Security patches (high)     | Within 1 week      | Tech lead review         |
| Minor version updates       | Weekly batch       | Automated PR review      |
| Major version updates       | Quarterly planning | Architecture review      |
| Development dependencies    | Monthly            | Team lead approval       |

### Breaking Change Strategy

**Major Version Update Process**:

1. **Impact Assessment** (Week 1)
   - Review changelog and migration guides
   - Identify affected code areas
   - Estimate effort and risk
   - Create migration plan

2. **Proof of Concept** (Week 2)
   - Create feature branch
   - Update in isolated subsystem
   - Validate migration approach
   - Document blockers or surprises

3. **Incremental Migration** (Weeks 3-N)
   - Migrate one module/service at a time
   - Maintain parallel compatibility where possible
   - Use adapter patterns for gradual transitions
   - Verify each migration with full test suite

4. **Deprecation Cleanup** (Week N+1)
   - Remove old version compatibility code
   - Update documentation
   - Archive migration notes

### Security Patches

**SLA by Severity**:

| Severity                 | Response Time | Testing Requirement        | Deployment Window     |
| ------------------------ | ------------- | -------------------------- | --------------------- |
| Critical (9.0-10.0 CVSS) | 4 hours       | Smoke tests                | Emergency deploy      |
| High (7.0-8.9 CVSS)      | 24 hours      | Full regression            | Next scheduled deploy |
| Medium (4.0-6.9 CVSS)    | 1 week        | Standard testing           | Regular release cycle |
| Low (0.1-3.9 CVSS)       | 1 month       | Batched with other updates | Monthly maintenance   |

**Security Patch Process**:

1. Automated vulnerability scanner creates PR
2. Security team triages and assigns severity
3. Development team tests and validates
4. Expedited review for high/critical severity
5. Post-deployment verification
6. Incident report for critical patches

### Compatibility Testing

**Pre-Update Validation**:

- ✅ All unit tests pass (100% existing tests)
- ✅ Integration tests pass (no new failures)
- ✅ Performance benchmarks within 5% baseline
- ✅ Security scans show no new vulnerabilities
- ✅ Build succeeds without warnings
- ✅ Deployment to staging environment succeeds

**Post-Update Monitoring** (First 48 Hours):

- Error rates < 0.1% increase
- Performance metrics within normal ranges
- No new security alerts
- User-reported issues < baseline

### Dependency Pinning vs Ranges

**When to Pin Exact Versions**:

- Production application dependencies
- Security-sensitive components
- After major version migrations (stabilization period)
- Binary/executable installations

**When to Use Version Ranges**:

- Library or package development (allow consumer flexibility)
- Development-only dependencies
- Patch-level updates (`~1.2.3` allows 1.2.x)
- Minor updates for stable libraries (`^1.2.3` allows 1.x)

**Configuration Examples**:

```json
// package.json - Application (pinned)
{
  "dependencies": {
    "react": "18.2.0",
    "axios": "1.6.2"
  }
}

// package.json - Library (ranges)
{
  "dependencies": {
    "lodash": "^4.17.0",
    "validator": "~13.11.0"
  },
  "peerDependencies": {
    "react": ">=16.8.0"
  }
}
```

```xml
<!-- .csproj - Pinned versions -->
<PackageReference Include="Microsoft.AspNetCore.App" Version="8.0.1" />
<PackageReference Include="Newtonsoft.Json" Version="13.0.3" />
```

### Tool Integration

**Recommended Tools**:

**Dependabot (GitHub)**:

```yaml
# .github/dependabot.yml
version: 2
updates:
  - package-ecosystem: "npm"
    directory: "/"
    schedule:
      interval: "weekly"
      day: "monday"
    open-pull-requests-limit: 5
    reviewers:
      - "tech-leads"
    labels:
      - "dependencies"
      - "automated"
    commit-message:
      prefix: "chore"
      include: "scope"
    groups:
      minor-patches:
        patterns:
          - "*"
        update-types:
          - "minor"
          - "patch"
```

**Renovate (Advanced Configuration)**:

```json
{
  "extends": ["config:base"],
  "schedule": ["before 10am on Monday"],
  "vulnerabilityAlerts": {
    "enabled": true,
    "labels": ["security"]
  },
  "packageRules": [
    {
      "matchUpdateTypes": ["major"],
      "automerge": false,
      "schedule": ["monthly"]
    },
    {
      "matchUpdateTypes": ["minor", "patch"],
      "automerge": true,
      "automergeType": "pr",
      "minimumReleaseAge": "3 days"
    }
  ]
}
```

## Code Modernization Practices

### Language Version Policy

**Upgrade Timeline**:

- **LTS (Long-Term Support) Versions**: Adopt within 3 months of release
- **Non-LTS Versions**: Evaluate for adoption, upgrade when next LTS available
- **EOL (End-of-Life) - 6 months**: Begin migration planning
- **EOL (End-of-Life) + 0 months**: Complete migration

**Current Language Status Examples** (as of 2026-02-10):

| Language  | Current Production | Target Version    | EOL Date   | Action Required          |
| --------- | ------------------ | ----------------- | ---------- | ------------------------ |
| C# / .NET | .NET 8             | .NET 9 (Nov 2024) | 2026-11-10 | Evaluate .NET 9 features |
| Python    | 3.11               | 3.12              | 2027-10    | Plan 3.12 migration      |
| Node.js   | 20 LTS             | 22 LTS (Apr 2024) | 2026-04-30 | Migrate to 22 LTS        |
| Java      | 17 LTS             | 21 LTS            | 2029-09    | Evaluate 21 LTS features |

**Migration Checklist**:

- [ ] Review language changelog for breaking changes
- [ ] Update build configuration and CI/CD pipelines
- [ ] Update IDE and linting configurations
- [ ] Test all critical paths with new version
- [ ] Update documentation and developer setup guides
- [ ] Train team on new language features
- [ ] Update coding standards to leverage new features

### Framework Updates

**Framework Update Strategy**:

1. **Monitor Release Cycles**
   - Subscribe to framework release notifications
   - Review roadmap for planned breaking changes
   - Identify features that solve current pain points

2. **Impact Analysis**
   - Calculate blast radius (files/components affected)
   - Identify deprecated API usage
   - Estimate effort (hours per major version)

3. **Migration Approach Selection**

   **Big Bang Migration** (Small projects, <10K LOC):
   - Dedicate 1-2 sprints to complete migration
   - Use feature branch for all changes
   - Comprehensive testing before merge

   **Strangler Fig Pattern** (Large projects, >10K LOC):
   - Maintain parallel versions using dependency injection
   - Migrate modules incrementally
   - Use abstraction layer to support both versions
   - Complete migration over multiple releases

4. **Testing Requirements**
   - 100% unit test pass rate required
   - Integration tests cover all critical user flows
   - Performance testing validates no regression
   - Load testing for high-traffic systems

**Framework-Specific Examples**:

**.NET Framework Updates**:

```xml
<!-- Upgrade from .NET 8 to .NET 9 -->
<Project Sdk="Microsoft.NET.Sdk.Web">
  <PropertyGroup>
    <TargetFramework>net9.0</TargetFramework>
    <!-- Enable latest language features -->
    <LangVersion>latest</LangVersion>
    <!-- Opt-in to nullability checks -->
    <Nullable>enable</Nullable>
  </PropertyGroup>
</Project>
```

**React Version Migration**:

```json
{
  "scripts": {
    "codemod": "npx react-codemod update-react-imports",
    "test:compat": "npm test -- --testNamePattern='legacy|deprecated'"
  }
}
```

### API Deprecation Handling

**Deprecation Timeline**:

1. **Announcement** (T-0): Deprecation notice with migration path
2. **Warning Period** (3-6 months): Warning logs, but API remains functional
3. **Error Period** (6-12 months): API throws errors in development, warns in production
4. **Removal** (12+ months): API fully removed from codebase

**Deprecation Detection**:

- Enable compiler warnings for deprecated APIs
- Use linters to flag deprecated patterns
- Monitor runtime deprecation warnings in logs
- Track deprecated API usage with telemetry

**Migration Process**:

```markdown
# Deprecation Migration Template

## Deprecated API

`OldClass.DeprecatedMethod(param1, param2)`

## Replacement API

`NewClass.ModernMethod(param1, param2, options)`

## Migration Steps

1. Identify all usages: `git grep -n "DeprecatedMethod"`
2. For each usage:
   - Replace with new API
   - Update tests
   - Verify functionality
3. Remove deprecated imports/references

## Breaking Changes

- `param2` type changed from `string` to `enum`
- New `options` parameter required for advanced features

## Timeline

- Announced: 2026-01-01
- Removal: 2026-07-01
```

### Pattern Updates

**Modern Pattern Adoption Checklist**:

**Async/Await (Replace Callbacks)**:

```csharp
// Old Pattern (Callback)
public void LoadData(Action<Data> callback) {
    // ... async work ...
    callback(result);
}

// Modern Pattern (Async/Await)
public async Task<Data> LoadDataAsync() {
    // ... async work with await ...
    return await GetResultAsync();
}
```

**Dependency Injection (Replace Service Locator)**:

```csharp
// Old Pattern (Service Locator)
public class MyService {
    private IRepo repo = ServiceLocator.Get<IRepo>();
}

// Modern Pattern (Constructor Injection)
public class MyService {
    private readonly IRepo _repo;
    public MyService(IRepo repo) {
        _repo = repo;
    }
}
```

**Pattern Modernization Process**:

1. Identify anti-patterns through code reviews
2. Document modern replacement pattern
3. Create automated refactoring scripts where possible
4. Migrate one module at a time
5. Update coding standards documentation

### Code Quality Standards

**Linting Configuration** (ESLint Example):

```json
{
  "extends": ["eslint:recommended", "plugin:@typescript-eslint/recommended"],
  "rules": {
    "no-console": "warn",
    "no-unused-vars": "error",
    "prefer-const": "error",
    "no-var": "error",
    "eqeqeq": ["error", "always"],
    "complexity": ["warn", 10],
    "max-lines": ["warn", 300]
  }
}
```

**Code Complexity Limits**:

- Cyclomatic complexity: ≤10 per function
- Function length: ≤50 lines
- File length: ≤300 lines
- Parameter count: ≤5 parameters
- Nesting depth: ≤3 levels

**Automated Formatting**:

- Enforce consistent formatting via Prettier, Black, gofmt, etc.
- Configure pre-commit hooks to auto-format
- Fail CI builds on formatting violations
- Zero tolerance for formatting debates (tool decides)

## Technical Debt Management

### Debt Identification Methods

**Code Analysis Tools**:

- **SonarQube**: Code smells, complexity, security hotspots
- **CodeClimate**: Maintainability ratings, duplication
- **NDepend**: .NET architecture and quality metrics
- **Roslyn Analyzers**: C# code quality and style

**Manual Identification**:

- Code review observations
- Performance bottlenecks
- Difficult-to-test code
- Frequent bug sources
- Developer pain points

**Debt Markers in Code**:

```csharp
// TODO-DEBT: This uses deprecated API, migrate to v2
// DEBT(ticket-123): Temporary workaround, needs proper solution
// FIXME-PERF: O(n²) algorithm, optimize for large datasets
// HACK: Remove after upstream library fixes bug
```

### Prioritization Framework

**Technical Debt Severity Matrix**:

| Impact \ Frequency                          | Rare    | Occasional | Frequent |
| ------------------------------------------- | ------- | ---------- | -------- |
| **High Impact** (Blocks features, security) | Medium  | High       | Critical |
| **Medium Impact** (Slows development, bugs) | Low     | Medium     | High     |
| **Low Impact** (Code smell, minor)          | Backlog | Low        | Medium   |

**Priority Definitions**:

- **Critical**: Address immediately (within sprint)
- **High**: Schedule in next 1-2 sprints
- **Medium**: Include in quarterly planning
- **Low**: Opportunistic (fix when touching nearby code)
- **Backlog**: Track but no scheduled work

**Cost-Benefit Analysis**:

```
Priority Score = (Impact × Frequency) / (Effort × Risk)

Where:
- Impact: 1-10 (business/developer impact)
- Frequency: 1-10 (how often encountered)
- Effort: 1-10 (hours/days to fix)
- Risk: 1-5 (risk of introducing bugs)
```

### Refactoring Guidelines

**When to Refactor**:

- ✅ Before adding new feature to complex code
- ✅ After identifying pattern in code review
- ✅ When tests are difficult to write
- ✅ During bug fix to prevent recurrence
- ✅ As part of scheduled debt reduction

**When NOT to Refactor**:

- ❌ Without test coverage (add tests first)
- ❌ During emergency bug fixes (separate PR)
- ❌ Mixing with feature development (separate commits)
- ❌ Legacy code not under active development

**Refactoring Process**:

1. **Establish Safety Net**: Ensure comprehensive test coverage
2. **Make Small Changes**: Refactor in small, verifiable steps
3. **Run Tests Frequently**: After each small change
4. **Commit Incrementally**: Commit each successful refactoring step
5. **Document Why**: Explain the improvement in commit message

**Safe Refactoring Techniques**:

- Extract Method/Function
- Rename Variable/Method
- Extract Interface/Abstract Class
- Move Method to more appropriate class
- Replace Magic Numbers with Constants
- Simplify Conditional Logic

### Code Review Standards

**Preventing New Debt in Reviews**:

**Checklist for Reviewers**:

- [ ] Code follows established patterns and conventions
- [ ] No duplicated logic from existing codebase
- [ ] Complexity metrics within acceptable ranges
- [ ] Tests included and cover edge cases
- [ ] No commented-out code (use git history instead)
- [ ] No TODOs without ticket references
- [ ] Documentation updated for public APIs
- [ ] Performance implications considered

**Debt-Introducing Red Flags**:

- Copy-pasted code blocks
- Deep nesting (>3 levels)
- Long functions (>50 lines)
- Commented-out code without explanation
- Hardcoded values without constants
- Broad exception catching without handling
- Missing error handling

### Metrics and Tracking

**Key Technical Debt Metrics**:

| Metric                      | Target        | Measurement Frequency |
| --------------------------- | ------------- | --------------------- |
| Code Duplication            | <3%           | Weekly                |
| Code Coverage               | >80%          | Per commit            |
| Cyclomatic Complexity (avg) | <5            | Weekly                |
| Technical Debt Ratio        | <5%           | Monthly               |
| Critical Issues             | 0             | Continuous            |
| Code Smells                 | Trending down | Weekly                |

**Debt Tracking Workflow**:

1. **Automated Detection**: Tools create debt tickets
2. **Manual Curation**: Team reviews and prioritizes weekly
3. **Sprint Allocation**: 15-20% sprint capacity to debt
4. **Progress Tracking**: Dashboard showing debt trends
5. **Retrospective Review**: Debt reduction as sprint goal

**Dashboard Example Elements**:

- Total debt items by priority
- Debt age distribution (how long it's existed)
- Debt introduction rate (new vs resolved)
- Module hotspots (areas with most debt)
- Trend lines (improving or degrading)

## Security and Vulnerability Management

### Vulnerability Scanning

**Tool Stack**:

**Dependency Scanning**:

- **Snyk**: Continuous monitoring with automated PR fixes
- **OWASP Dependency-Check**: Open-source alternative
- **GitHub Security Advisories**: Native GitHub integration
- **WhiteSource/Mend**: Enterprise license compliance

**Secret Detection**:

- **TruffleHog**: Git history scanning for secrets
- **GitGuardian**: Real-time secret detection
- **GitHub Secret Scanning**: Automatic token detection

**Code Analysis**:

- **Semgrep**: Custom security rules
- **CodeQL**: Advanced semantic analysis
- **Bandit** (Python), **Brakeman** (Ruby), **gosec** (Go)

**Scanning Frequency**:

- **Continuous**: On every commit (via CI/CD)
- **Daily**: Full dependency scan of all projects
- **Weekly**: Deep analysis with manual review
- **On-demand**: Before releases or security audits

**Configuration Example**:

```yaml
# .github/workflows/security-scan.yml
name: Security Scan
on:
  push:
    branches: [main]
  pull_request:
  schedule:
    - cron: "0 6 * * 1" # Weekly Monday 6 AM

jobs:
  security:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
        with:
          fetch-depth: 0 # Full history for secret scan

      - name: Run TruffleHog
        uses: trufflesecurity/trufflehog@main
        with:
          path: ./
          base: ${{ github.event.pull_request.base.sha }}
          head: HEAD
          extra_args: --only-verified

      - name: Run Snyk Security Scan
        uses: snyk/actions/node@master
        env:
          SNYK_TOKEN: ${{ secrets.SNYK_TOKEN }}
        with:
          args: --severity-threshold=high

      - name: OWASP Dependency Check
        uses: dependency-check/Dependency-Check_Action@main
        with:
          project: "project-name"
          path: "."
          format: "HTML"
```

### SBOM Requirements

**Software Bill of Materials (SBOM) Definition**:
An SBOM is a complete inventory of all components, libraries, and dependencies in your software, including:

- Component names and versions
- Licenses
- Dependencies and relationships
- Known vulnerabilities

**SBOM Standards**:

- **CycloneDX**: Lightweight, purpose-built for security
- **SPDX**: Linux Foundation standard, detailed provenance

**Generation Requirements**:

- Generate SBOM for every release
- Include in release artifacts
- Store in searchable repository
- Update when dependencies change

**SBOM Generation Example**:

```yaml
# Generate CycloneDX SBOM
- name: Generate SBOM
  uses: CycloneDX/gh-action@v2
  with:
    version: v1
    args: "generate -o sbom.json -t application"

- name: Upload SBOM
  uses: actions/upload-artifact@v3
  with:
    name: sbom
    path: sbom.json
    retention-days: 90

- name: Submit SBOM to Dependency Track
  run: |
    curl -X POST "https://dtrack.company.com/api/v1/bom" \
      -H "X-API-Key: ${{ secrets.DTRACK_API_KEY }}" \
      -H "Content-Type: multipart/form-data" \
      -F "project=${{ github.repository }}" \
      -F "bom=@sbom.json"
```

### Secret Detection

**Secret Types to Detect**:

- API keys and tokens
- Database credentials
- Private keys and certificates
- OAuth tokens
- Encryption keys
- Cloud service credentials

**Prevention Strategies**:

1. **Pre-commit Hooks**: Block secrets before commit
2. **CI/CD Gates**: Scan all commits and PRs
3. **Periodic Scans**: Full repository history checks
4. **Training**: Educate developers on secret management

**Pre-commit Hook Example**:

```bash
#!/bin/bash
# .git/hooks/pre-commit

# Run TruffleHog on staged files
if command -v trufflehog &> /dev/null; then
    echo "Scanning for secrets..."

    # Get list of staged files
    staged_files=$(git diff --cached --name-only)

    if [ -n "$staged_files" ]; then
        trufflehog git file://. --since-commit HEAD --only-verified --fail

        if [ $? -ne 0 ]; then
            echo "❌ Secret detected! Commit blocked."
            echo "Remove the secret and try again."
            exit 1
        fi
    fi
fi

echo "✅ No secrets detected"
```

**Remediation Process**:

1. **Immediate**: Revoke exposed credential
2. **Within 1 hour**: Remove from git history (`git filter-branch` or BFG)
3. **Within 4 hours**: Generate new credential
4. **Within 24 hours**: Review and rotate related credentials
5. **Post-incident**: Document and conduct team training

### Dependency Audit Trails

**Audit Log Requirements**:

- Track all dependency changes (additions, updates, removals)
- Record who approved major version updates
- Document security exceptions and justifications
- Maintain SBOM history for all releases

**Audit Workflow**:

```markdown
# Dependency Change Request

**Package**: lodash
**Current Version**: 4.17.19
**Proposed Version**: 4.17.21
**Change Type**: Minor update
**Reason**: Security patch for CVE-2021-23337

## Security Impact

- Fixes prototype pollution vulnerability
- No known breaking changes
- Backward compatible

## Testing Completed

- [x] Unit tests pass
- [x] Integration tests pass
- [x] Manual testing in staging

## Approval

- Tech Lead: @tech-lead (approved)
- Security Team: @security-team (approved)
- Date: 2026-02-10
```

### Security Update SLAs

| Severity | CVSS Score | Discovery to PR | PR to Merge | Merge to Deploy | Total SLA |
| -------- | ---------- | --------------- | ----------- | --------------- | --------- |
| Critical | 9.0-10.0   | 2 hours         | 2 hours     | 4 hours         | 8 hours   |
| High     | 7.0-8.9    | 8 hours         | 16 hours    | 24 hours        | 48 hours  |
| Medium   | 4.0-6.9    | 3 days          | 4 days      | 7 days          | 14 days   |
| Low      | 0.1-3.9    | 2 weeks         | 2 weeks     | 4 weeks         | 8 weeks   |

**SLA Accountability**:

- **Critical/High**: Requires executive notification if SLA breached
- **Medium**: Requires team lead notification
- **Low**: Tracked but no escalation

**Exception Process**:

- Document why SLA cannot be met
- Provide mitigation measures (firewall rules, feature flags)
- Get security team approval for delay
- Schedule follow-up remediation

## Testing and Quality Assurance

### Test Evolution

**Test Maintenance Requirements**:

- Update tests when code changes (no orphaned tests)
- Remove tests for deleted features
- Refactor duplicated test logic
- Keep test data realistic and up-to-date
- Update mocks when API changes

**Test Smell Detection**:

- **Fragile Tests**: Break frequently without code changes
- **Slow Tests**: Take >100ms for unit tests
- **Test Interdependence**: Tests fail when run in different order
- **Obscure Tests**: Behavior isn't clear from test name
- **Assertion Roulette**: Multiple assertions without clear meaning

**Test Refactoring Priority**:

1. Failing tests (immediate fix)
2. Flaky tests (investigate and stabilize)
3. Slow tests (optimize or move to integration suite)
4. Duplicate test logic (extract helpers)
5. Unclear tests (improve naming and structure)

### Coverage Requirements

**Minimum Coverage Thresholds**:

| Code Type         | Minimum Coverage | Target Coverage |
| ----------------- | ---------------- | --------------- |
| Business Logic    | 80%              | 90%+            |
| Utility Functions | 90%              | 95%+            |
| API Endpoints     | 70%              | 85%+            |
| UI Components     | 60%              | 75%+            |
| Configuration     | 50%              | N/A             |

**Coverage Configuration**:

```json
// jest.config.js
{
  "coverageThresholds": {
    "global": {
      "branches": 80,
      "functions": 80,
      "lines": 80,
      "statements": 80
    },
    "./src/core/**/*.ts": {
      "branches": 90,
      "functions": 95,
      "lines": 95,
      "statements": 95
    }
  }
}
```

**Coverage CI Enforcement**:

```yaml
- name: Test with Coverage
  run: npm test -- --coverage

- name: Coverage Check
  run: |
    COVERAGE=$(jq '.total.lines.pct' coverage/coverage-summary.json)
    if (( $(echo "$COVERAGE < 80" | bc -l) )); then
      echo "❌ Coverage $COVERAGE% is below 80% threshold"
      exit 1
    fi
    echo "✅ Coverage: $COVERAGE%"
```

**What NOT to Test**:

- ❌ Third-party library internals
- ❌ Auto-generated code (unless business logic)
- ❌ Simple getters/setters without logic
- ❌ Configuration files
- ❌ Trivial one-line functions

### Performance Testing

**Performance Metrics to Track**:

- Response time (p50, p95, p99)
- Throughput (requests per second)
- Resource usage (CPU, memory, I/O)
- Database query performance
- API latency

**Regression Detection**:

```yaml
# Performance test in CI
- name: Run Performance Tests
  run: npm run test:performance

- name: Compare with Baseline
  run: |
    python scripts/compare-performance.py \
      --current results/performance.json \
      --baseline baseline/performance.json \
      --threshold 10  # Fail if >10% regression
```

**Performance Testing Tools**:

- **Artillery**: Load testing for web services
- **k6**: Modern load testing tool with JS scripting
- **JMeter**: Enterprise-grade load testing
- **autocannon**: Node.js HTTP benchmarking
- **BenchmarkDotNet**: .NET performance benchmarking

### Integration Testing

**External Dependency Validation**:

- Test against real external APIs in staging
- Use contract testing (Pact) for API consumers
- Mock external services in unit tests
- Validate API responses match expected schema
- Test error handling for service failures

**Test Environment Management**:

```yaml
# docker-compose.test.yml
version: "3.8"
services:
  app:
    build: .
    environment:
      - NODE_ENV=test
      - DATABASE_URL=postgresql://test:test@db:5432/test
    depends_on:
      - db
      - redis

  db:
    image: postgres:16
    environment:
      POSTGRES_DB: test
      POSTGRES_USER: test
      POSTGRES_PASSWORD: test

  redis:
    image: redis:7-alpine
```

### Test Maintenance

**Removing Obsolete Tests**:

- Delete tests for removed features (same PR)
- Archive tests for deprecated features (marked as skipped)
- Remove duplicate tests during refactoring
- Document why tests are removed in commit message

**Test Organization**:

```
tests/
├── unit/                  # Fast, isolated tests
│   ├── services/
│   ├── utils/
│   └── models/
├── integration/           # Tests with real dependencies
│   ├── api/
│   └── database/
├── e2e/                   # Full user flows
│   └── critical-paths/
├── performance/           # Load and benchmark tests
└── fixtures/              # Shared test data
    ├── mocks/
    └── samples/
```

## Documentation Currency

### API Documentation

**Auto-Generation Requirements**:

- Generate API docs from code annotations
- Include in CI/CD pipeline
- Publish on every merge to main
- Version documentation alongside code

**OpenAPI/Swagger Example**:

```csharp
/// <summary>
/// Creates a new user account
/// </summary>
/// <param name="request">User registration details</param>
/// <returns>Created user with unique ID</returns>
/// <response code="201">User created successfully</response>
/// <response code="400">Invalid input data</response>
/// <response code="409">Email already exists</response>
[HttpPost]
[ProducesResponseType(StatusCodes.Status201Created)]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
[ProducesResponseType(StatusCodes.Status409Conflict)]
public async Task<ActionResult<UserDto>> CreateUser(CreateUserRequest request)
{
    // Implementation
}
```

**Documentation CI Step**:

```yaml
- name: Generate API Documentation
  run: |
    dotnet tool install -g Swashbuckle.AspNetCore.Cli
    dotnet swagger tofile --output docs/api.json \
      bin/Release/net9.0/MyApp.dll v1

- name: Publish Documentation
  run: |
    npx redoc-cli bundle docs/api.json -o docs/api.html
    aws s3 cp docs/api.html s3://docs.company.com/api/latest/
```

### README Maintenance

**README Review Cycle**:

- Review quarterly for accuracy
- Update after major feature releases
- Validate setup instructions with new developer
- Keep badges current (build status, coverage, etc.)

**Required README Sections**:

```markdown
# Project Name

## Description

[One-paragraph overview]

## Prerequisites

[Software versions required]

## Installation

[Step-by-step setup]

## Usage

[Common commands and examples]

## Configuration

[Environment variables and settings]

## Testing

[How to run tests]

## Deployment

[Deployment instructions]

## Contributing

[How to contribute]

## License

## AI-Assisted Artifacts

[Links to AI-generated content with chat logs]
```

### Code Comments

**When to Update Comments**:

- ✅ When refactoring commented code
- ✅ When fixing bugs related to misunderstood logic
- ✅ When API behavior changes
- ✅ When assumptions become invalid

**When to Remove Comments**:

- ❌ Commented-out code (use git history instead)
- ❌ Obvious comments ("increment i by 1")
- ❌ Outdated comments contradicting code
- ❌ TODO comments older than 6 months without tickets

**Comment Quality Standards**:

```csharp
// ❌ Bad: Obvious
// Loop through users
foreach (var user in users) { }

// ✅ Good: Explains WHY
// Process users in batches to avoid memory exhaustion
foreach (var user in users) { }

// ❌ Bad: Outdated
// TODO: Fix this hack (written 2 years ago)

// ✅ Good: Linked to tracking
// TODO(JIRA-123): Refactor after API v2 migration
```

### Architecture Docs

**Keeping Diagrams Current**:

- Store diagrams as code (Mermaid, PlantUML, C4)
- Version control with application code
- Regenerate on every release
- Link from README to architecture docs

**Architecture Decision Records (ADRs)**:

```markdown
# ADR-001: Use PostgreSQL for Primary Database

## Status

Accepted (2026-01-15)

## Context

Need to choose primary database for application.

## Decision

Use PostgreSQL 16 with TimescaleDB extension.

## Consequences

### Positive

- ACID compliance for financial data
- Rich query capabilities with JSON support
- Strong ecosystem and tooling

### Negative

- Higher operational complexity than managed NoSQL
- Requires PostgreSQL expertise

## Alternatives Considered

- MongoDB: Rejected due to weak consistency
- MySQL: Rejected due to limited JSON support
```

### Change Documentation

**Changelog Format** (Keep-A-Changelog):

```markdown
# Changelog

## [Unreleased]

### Added

- User profile photo upload feature (#234)

### Changed

- Improved API response times by 40% (#245)

### Deprecated

- /api/v1/users endpoint (use /api/v2/users) (#250)

### Removed

- Legacy authentication method (#248)

### Fixed

- Email validation bug for special characters (#251)

### Security

- Patched XSS vulnerability in comments (#253)

## [2.1.0] - 2026-02-01

### Added

- Two-factor authentication support (#200)
```

**Release Notes Automation**:

```yaml
- name: Generate Release Notes
  uses: release-drafter/release-drafter@v5
  with:
    config-name: release-drafter.yml
  env:
    GITHUB_TOKEN: ${{ secrets.GITHUB_TOKEN }}
```

## CI/CD and Automation

### Pipeline Modernization

**Regular Pipeline Reviews**:

- Quarterly review of all CI/CD pipelines
- Annual evaluation of CI/CD platform features
- Monthly review of build performance metrics
- Weekly review of failed/flaky builds

**Pipeline Health Metrics**:

- Build success rate (target: >95%)
- Average build duration (track trends)
- Flaky test rate (target: <1%)
- Pipeline uptime (target: 99.5%)

**Modernization Opportunities**:

- Parallel job execution
- Build caching strategies
- Matrix builds for multiple platforms
- Self-hosted runners for better performance
- Pipeline-as-code best practices

### Tool Upgrades

**CI/CD Platform Updates**:
| Platform | Update Check Frequency | Update SLA |
|----------|----------------------|-----------|
| GitHub Actions | Monthly | Within 1 month of release |
| Azure DevOps | Quarterly | Within 2 months of release |
| Jenkins | Quarterly | Within 3 months of release |
| CircleCI | Monthly | Within 1 month of release |

**Action/Task Version Management**:

```yaml
# Use version pinning with automatic updates
- uses: actions/checkout@v4 # Major version pin
- uses: actions/setup-node@v4.0.2 # Exact version for reproducibility

# Renovate will create PRs to update these
```

### Build Optimization

**Optimization Techniques**:

**1. Dependency Caching**:

```yaml
- name: Cache Node Modules
  uses: actions/cache@v4
  with:
    path: ~/.npm
    key: ${{ runner.os }}-node-${{ hashFiles('**/package-lock.json') }}
    restore-keys: |
      ${{ runner.os }}-node-
```

**2. Parallel Execution**:

```yaml
jobs:
  test:
    strategy:
      matrix:
        node: [18, 20, 22]
        os: [ubuntu-latest, windows-latest]
    runs-on: ${{ matrix.os }}
    steps:
      - uses: actions/setup-node@v4
        with:
          node-version: ${{ matrix.node }}
```

**3. Conditional Steps**:

```yaml
- name: Run expensive tests
  if: github.event_name == 'push' && github.ref == 'refs/heads/main'
  run: npm run test:e2e
```

**Build Time Targets**:

- Unit tests: <5 minutes
- Integration tests: <15 minutes
- Full build + deploy: <30 minutes
- Emergency hotfix: <10 minutes

### Automated Checks

**Quality Gates (All must pass)**:

```yaml
name: Quality Gates

jobs:
  quality-gate:
    runs-on: ubuntu-latest
    steps:
      # 1. Linting
      - name: Lint Code
        run: npm run lint
        continue-on-error: false

      # 2. Unit Tests
      - name: Unit Tests
        run: npm test
        continue-on-error: false

      # 3. Coverage Check
      - name: Coverage Threshold
        run: |
          npm test -- --coverage
          if [ $(jq '.total.lines.pct' coverage/coverage-summary.json) < 80 ]; then
            exit 1
          fi

      # 4. Security Scan
      - name: Security Vulnerabilities
        run: npm audit --audit-level=high
        continue-on-error: false

      # 5. Build Success
      - name: Build Application
        run: npm run build
        continue-on-error: false

      # 6. Integration Tests
      - name: Integration Tests
        run: npm run test:integration
        continue-on-error: false

      # 7. Performance Baseline
      - name: Performance Test
        run: npm run test:performance
        continue-on-error: false
```

**Branch Protection Rules**:

- Require status checks to pass before merging
- Require up-to-date branches before merge
- Require code review from code owners
- Dismiss stale approvals on new commits
- Require linear history (no merge commits)

### Deployment Strategies

**Blue-Green Deployment**:

```yaml
deploy:
  - name: Deploy to Green Environment
    run: kubectl set image deployment/app app=app:${{ github.sha }} --namespace=green

  - name: Wait for Green Healthy
    run: kubectl wait --for=condition=ready pod -l app=myapp --namespace=green --timeout=300s

  - name: Run Smoke Tests
    run: npm run test:smoke -- --base-url=https://green.internal.company.com

  - name: Switch Traffic to Green
    run: kubectl patch service app -p '{"spec":{"selector":{"version":"green"}}}'

  - name: Monitor for 10 minutes
    run: ./scripts/monitor-deployment.sh --duration=600

  - name: Rollback on failure
    if: failure()
    run: kubectl patch service app -p '{"spec":{"selector":{"version":"blue"}}}'
```

**Canary Deployment**:

- Route 5% of traffic to new version
- Monitor error rates for 1 hour
- Gradually increase to 25%, 50%, 75%, 100%
- Automatic rollback if error threshold exceeded

**Feature Flags**:

```csharp
// Use feature flags for risky changes
if (await _featureFlag.IsEnabledAsync("new-payment-flow", user)) {
    return await ProcessPaymentV2(request);
} else {
    return await ProcessPaymentV1(request);
}
```

## Infrastructure Evolution

### Container Base Images

**Base Image Update Policy**:

- **Security patches**: Update within 7 days
- **Minor versions**: Update monthly
- **Major versions**: Evaluate quarterly, migrate within 6 months

**Image Version Pinning**:

```dockerfile
# ❌ Avoid: Latest tag (unpredictable)
FROM node:latest

# ⚠️ Acceptable: Major version
FROM node:20

# ✅ Best: Specific version with digest
FROM node:20.11.1-alpine3.19@sha256:abc123...

# Update with Renovate or Dependabot
```

**Dockerfile Best Practices**:

```dockerfile
# Use official images from trusted sources
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base

# Create non-root user
RUN adduser --disabled-password --gecos '' appuser
USER appuser

# Use multi-stage builds
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["app.csproj", "./"]
RUN dotnet restore
COPY . .
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

# Final stage - minimal
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "app.dll"]
```

**Automated Image Updates**:

```yaml
# Renovate configuration for Docker
{
  "dockerfile": { "enabled": true, "pinDigests": true },
  "packageRules":
    [
      {
        "matchDatasources": ["docker"],
        "matchUpdateTypes": ["major"],
        "enabled": false  // Manual review for major updates,
      },
    ],
}
```

### Cloud Service Versions

**Cloud Service Update Strategy**:

| Service Type            | Update Approach               | Testing Required      |
| ----------------------- | ----------------------------- | --------------------- |
| Managed Databases       | Blue-green with read replicas | Load testing          |
| Container Orchestration | Rolling update                | Progressive rollout   |
| Serverless Functions    | Versioned deployment          | Canary testing        |
| Message Queues          | Gray deployment               | End-to-end testing    |
| Storage Services        | API version updates           | Compatibility testing |

**AWS Service Example**:

```terraform
# RDS Major Version Upgrade
resource "aws_db_instance" "main" {
  engine_version = "16.1"  # PostgreSQL version
  allow_major_version_upgrade = false  # Explicit control
  auto_minor_version_upgrade = true    # Automatic patches

  # Blue-green deployment support
  blue_green_update {
    enabled = true
  }
}
```

**Azure Service Example**:

```bicep
// Azure SQL Database
resource sqlDatabase 'Microsoft.Sql/servers/databases@2023-05-01' = {
  name: 'production-db'
  properties: {
    collation: 'SQL_Latin1_General_CP1_CI_AS'
    maxSizeBytes: 268435456000
    autoPauseDelay: -1
    minCapacity: 2
    // Automatic tuning
    automaticTuning: {
      mode: 'Auto'
    }
  }
}
```

### Database Migrations

**Migration Versioning Strategy**:

- Sequential versioning (001, 002, 003)
- Timestamp-based versioning (20260210_123045)
- Semantic versioning for schema (major.minor.patch)

**Backward Compatibility Rules**:

1. **Additive Changes** (Safe): Add columns, tables, indexes
2. **Modify Existing** (Requires Planning):
   - Add column (nullable or with default)
   - Deploy code supporting both old and new schema
   - Backfill data if needed
   - Deploy code using only new schema
   - Remove old schema elements

3. **Destructive Changes** (High Risk):
   - Must maintain old schema temporarily
   - Provide migration scripts
   - Require rollback plan

**Migration Example (Entity Framework)**:

```csharp
// Step 1: Add nullable column
public class AddEmailVerificationColumn : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<DateTime?>(
            name: "EmailVerifiedAt",
            table: "Users",
            nullable: true);
    }
}

// Step 2: Backfill existing data
public class BackfillEmailVerification : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql(@"
            UPDATE Users
            SET EmailVerifiedAt = CreatedAt
            WHERE EmailVerifiedAt IS NULL
            AND EmailConfirmed = 1
        ");
    }
}

// Step 3: Make column non-nullable
public class MakeEmailVerificationRequired : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<DateTime>(
            name: "EmailVerifiedAt",
            table: "Users",
            nullable: false);
    }
}
```

### Configuration Management

**Infrastructure as Code Updates**:

- Version control all IaC (Terraform, Bicep, CloudFormation)
- Test infrastructure changes in staging
- Use modules/templates for reusability
- Pin provider versions

**Terraform Version Management**:

```hcl
terraform {
  required_version = "~> 1.7.0"  # Allow patch updates

  required_providers {
    aws = {
      source  = "hashicorp/aws"
      version = "~> 5.0"
    }
  }

  backend "s3" {
    bucket = "terraform-state"
    key    = "prod/terraform.tfstate"
    region = "us-east-1"

    # State locking
    dynamodb_table = "terraform-lock"
  }
}
```

**Configuration Drift Detection**:

```yaml
# Daily drift detection
- name: Terraform Plan
  run: terraform plan -detailed-exitcode
  id: plan
  continue-on-error: true

- name: Notify on Drift
  if: steps.plan.outputs.exitcode == 2
  run: |
    echo "⚠️ Infrastructure drift detected!"
    terraform show -no-color > drift-report.txt
    # Send notification to team
```

### Monitoring and Observability

**Telemetry Evolution**:

- Add new metrics when patterns emerge
- Remove metrics no longer providing value
- Update alert thresholds based on historical data
- Refresh dashboards quarterly

**Structured Logging Standards**:

```csharp
// Use structured logging
_logger.LogInformation(
    "User {UserId} performed {Action} on {Resource}",
    userId, action, resourceId);

// Not: String concatenation
_logger.LogInformation($"User {userId} performed {action}");
```

**Observability Stack Updates**:

```yaml
# Update observability tools
monitoring:
  prometheus:
    version: "2.50.0"
    retention: "30d"

  grafana:
    version: "10.3.0"
    plugins:
      - grafana-piechart-panel
      - grafana-worldmap-panel

  loki:
    version: "2.9.0"
    retention: "14d"

  tempo:
    version: "2.3.0"
```

## Compliance and Enforcement

### PR Requirements

**Mandatory PR Checks**:

All PRs must pass these checks before merge:

✅ **Build Status**: All CI jobs pass
✅ **Test Coverage**: Coverage ≥80% and no decrease
✅ **Security Scan**: No high/critical vulnerabilities
✅ **Linting**: All linting rules pass
✅ **Code Review**: Approved by at least 1 code owner
✅ **Dependency Audit**: No known security issues in dependencies
✅ **Documentation**: README updated if new features added
✅ **Changelog**: Entry added for user-facing changes

**PR Template**:

```markdown
## Description

[Brief description of changes]

## Type of Change

- [ ] Bug fix (non-breaking change fixing an issue)
- [ ] New feature (non-breaking change adding functionality)
- [ ] Breaking change (fix or feature causing existing functionality to change)
- [ ] Documentation update

## Checklist

- [ ] Tests added/updated
- [ ] Documentation updated
- [ ] No security vulnerabilities introduced
- [ ] Dependencies updated if needed
- [ ] Breaking changes documented
- [ ] Performance impact considered

## AI-Assisted Content

- [ ] No AI-assisted content
- [ ] AI-assisted content with provenance metadata
  - Chat log: [link]
  - Summary: [link]
```

### CI Gates

**Blocking Conditions**:

```yaml
# .github/workflows/gates.yml
name: Quality Gates

on:
  pull_request:
  push:
    branches: [main]

jobs:
  # Gate 1: Security
  security:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4

      - name: Vulnerability Scan
        run: npm audit --audit-level=high
        # Blocks on high/critical vulnerabilities

      - name: Secret Scan
        uses: trufflesecurity/trufflehog@main
        # Blocks on detected secrets

  # Gate 2: Quality
  quality:
    runs-on: ubuntu-latest
    steps:
      - name: Lint
        run: npm run lint
        # Blocks on linting errors

      - name: Type Check
        run: npm run type-check
        # Blocks on type errors

  # Gate 3: Tests
  tests:
    runs-on: ubuntu-latest
    steps:
      - name: Unit Tests
        run: npm test
        # Blocks on test failures

      - name: Coverage Gate
        run: |
          npm test -- --coverage
          COVERAGE=$(jq '.total.lines.pct' coverage/coverage-summary.json)
          if (( $(echo "$COVERAGE < 80" | bc -l) )); then
            echo "Coverage $COVERAGE% below threshold"
            exit 1
          fi

  # Gate 4: Build
  build:
    runs-on: ubuntu-latest
    steps:
      - name: Build Application
        run: npm run build
        # Blocks on build failures

  # Require all gates to pass
  all-gates:
    needs: [security, quality, tests, build]
    runs-on: ubuntu-latest
    steps:
      - run: echo "All quality gates passed ✅"
```

### Audit Reports

**Regular Compliance Reviews**:

| Report Type              | Frequency   | Owner                | Output              |
| ------------------------ | ----------- | -------------------- | ------------------- |
| Security Vulnerabilities | Daily       | Security Team        | Email summary       |
| Dependency Freshness     | Weekly      | Tech Leads           | Dashboard           |
| Code Quality Trends      | Weekly      | Engineering Managers | Report              |
| Technical Debt           | Monthly     | Tech Leads           | Prioritized backlog |
| SBOM Validation          | Per Release | Release Manager      | Release artifact    |
| Performance Metrics      | Monthly     | DevOps               | Trend analysis      |

**Automated Reporting Example**:

```python
# scripts/generate-compliance-report.py

import json
from datetime import datetime

def generate_report():
    report = {
        "report_date": datetime.now().isoformat(),
        "metrics": {
            "vulnerabilities": get_vulnerability_count(),
            "outdated_dependencies": get_outdated_count(),
            "code_coverage": get_coverage_percentage(),
            "technical_debt_hours": estimate_debt_hours(),
            "build_success_rate": get_build_success_rate()
        },
        "compliance_status": {
            "security_patches": check_patch_sla(),
            "dependency_updates": check_update_policy(),
            "test_coverage": check_coverage_threshold()
        }
    }

    # Generate report
    with open('compliance-report.json', 'w') as f:
        json.dump(report, f, indent=2)

    # Send notifications if non-compliant
    if not all(report["compliance_status"].values()):
        notify_team(report)
```

### Exception Process

**When to Request Exception**:

- Unable to update dependency due to breaking changes
- Security patch conflicts with critical functionality
- Technical constraint (legacy system compatibility)
- Business priority conflicts with maintenance

**Exception Request Template**:

```markdown
# Evergreen Compliance Exception Request

## Request Details

- **Date**: 2026-02-10
- **Requestor**: @developer-name
- **Approver Required**: Tech Lead + Security Team

## Non-Compliance Item

- **Type**: Security patch delay
- **Package**: lodash
- **Current Version**: 4.17.19
- **Required Version**: 4.17.21
- **Policy Violation**: Security patch SLA (7 days)

## Justification

Breaking change in 4.17.21 requires significant refactoring
of validation logic used across 45 files.

## Risk Assessment

- **Vulnerability**: CVE-2021-23337 (Prototype Pollution)
- **CVSS Score**: 7.4 (High)
- **Exploitability**: Low (requires specific conditions)
- **Current Mitigation**: Input validation prevents exploitation

## Mitigation Plan

1. Implement additional input sanitization (completed)
2. Add monitoring for exploitation attempts (in progress)
3. Schedule refactoring for next sprint (2 weeks)
4. Apply patch immediately after refactoring

## Timeline

- **Exception Duration**: 14 days
- **Remediation Date**: 2026-02-24
- **Follow-up Review**: 2026-02-25

## Approval

- [ ] Tech Lead: ******\_\_\_\_****** Date: **\_\_**
- [ ] Security Team: ****\_\_\_\_**** Date: **\_\_**
```

**Exception Tracking**:

- Log all exceptions in tracking system
- Set calendar reminders for remediation dates
- Review open exceptions in weekly sync
- Escalate overdue exceptions to leadership

### Remediation Deadlines

**Violation Severity Timelines**:

| Severity                | Detection to Remediation | Escalation Threshold |
| ----------------------- | ------------------------ | -------------------- |
| **Critical Security**   | 48 hours                 | 24 hours             |
| **High Security**       | 7 days                   | 4 days               |
| **Medium Security**     | 30 days                  | 21 days              |
| **Low Security**        | 90 days                  | 60 days              |
| **Quality Gate**        | Next sprint              | 2 sprints            |
| **Outdated Dependency** | 90 days                  | 60 days              |
| **Technical Debt**      | Prioritized backlog      | N/A                  |

**Escalation Process**:

1. **Warning**: Automated notification to developer + team lead
2. **First Escalation**: Manager notification + required action plan
3. **Second Escalation**: Director notification + sprint commitment
4. **Final Escalation**: VP notification + dedicated sprint allocation

**Remediation Tracking Dashboard**:

```markdown
# Open Violations Dashboard

| ID    | Type         | Severity | Age     | Owner | Status      | Due Date   |
| ----- | ------------ | -------- | ------- | ----- | ----------- | ---------- |
| V-001 | Security     | High     | 3 days  | @dev1 | In Progress | 2026-02-13 |
| V-002 | Outdated Dep | Medium   | 45 days | @dev2 | Blocked     | 2026-03-01 |
| V-003 | Quality      | Low      | 12 days | @dev3 | Not Started | 2026-02-20 |

## Overdue Items (Escalated)

- V-004: Critical security patch (2 days overdue) - **EXECUTIVE ATTENTION**
```

## Practical Examples

### Example A: Dependency Update Workflow

**Automated Dependabot PR**:

```yaml
# .github/dependabot.yml
version: 2
updates:
  - package-ecosystem: "npm"
    directory: "/"
    schedule:
      interval: "weekly"
    commit-message:
      prefix: "chore(deps)"
    reviewers:
      - "backend-team"
    labels:
      - "dependencies"
      - "automated"
```

**PR Checklist for Dependency Updates**:

```markdown
## Dependency Update: lodash 4.17.19 → 4.17.21

### Change Summary

- **Type**: Security patch
- **Changelog**: https://github.com/lodash/lodash/blob/main/CHANGELOG.md
- **Breaking Changes**: None
- **New Features**: N/A

### Testing Completed

- [x] All unit tests pass (1,234 tests)
- [x] Integration tests pass (156 tests)
- [x] Manual smoke testing in dev environment
- [x] No new TypeScript errors
- [x] Build succeeds without warnings

### Security Impact

- **Fixes**: CVE-2021-23337 (Prototype Pollution)
- **Severity**: High (CVSS 7.4)
- **New Vulnerabilities**: None detected

### Performance Impact

- Bundle size: No change
- Build time: -2 seconds (faster)
- Test execution: No change

### Approval Required

- [x] Automated tests (GitHub Actions)
- [ ] Code owner review (@tech-lead)
- [x] Security scan (Snyk)

### Auto-merge Eligibility

✅ Eligible for auto-merge (minor version, tests pass, no breaking changes)
```

### Example B: Code Modernization Template

**Migration Plan: C# 11 to C# 12**

````markdown
# C# 12 Migration Plan

## Objective

Upgrade from C# 11 to C# 12 to leverage new language features
and maintain language version currency.

## Timeline

- **Start Date**: 2026-02-15
- **Completion Date**: 2026-03-15 (4 weeks)
- **Phases**: 4 phases, 1 week each

## Phase 1: Preparation (Week 1)

### Tasks

- [x] Review C# 12 changelog and breaking changes
- [x] Update .NET SDK to 9.0
- [x] Update CI/CD pipelines to use .NET 9
- [ ] Run build with `<LangVersion>12</LangVersion>`
- [ ] Document all compiler errors/warnings

### Deliverables

- Build succeeds with C# 12
- List of modernization opportunities

## Phase 2: Core Libraries (Week 2)

### Scope

- Update utility libraries
- Update data models
- Update service interfaces

### Modernization Opportunities

**Primary Constructors**:

```csharp
// Before (C# 11)
public class UserService
{
    private readonly IRepo _repo;
    private readonly ILogger _logger;

    public UserService(IRepo repo, ILogger logger)
    {
        _repo = repo;
        _logger = logger;
    }
}

// After (C# 12)
public class UserService(IRepo repo, ILogger logger)
{
    public async Task<User> GetUser(int id)
    {
        logger.LogInformation("Getting user {Id}", id);
        return await repo.GetUserAsync(id);
    }
}
```
````

**Collection Expressions**:

```csharp
// Before
var numbers = new List<int> { 1, 2, 3 };
var combined = new List<int>();
combined.AddRange(numbers);
combined.Add(4);

// After
int[] numbers = [1, 2, 3];
var combined = [..numbers, 4];
```

### Testing Requirements

- 100% unit test pass rate
- No performance regression
- Code review for readability

## Phase 3: API Layer (Week 3)

### Scope

- Update controllers
- Update middleware
- Update request/response models

### Modernization Opportunities

**Interceptors (for logging)**:

```csharp
// Apply cross-cutting concerns
[Logging]
public class UserController : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUser(int id)
    {
        // Logging handled by interceptor
        return await _service.GetUserAsync(id);
    }
}
```

## Phase 4: Validation and Cleanup (Week 4)

### Tasks

- [ ] Full regression test suite
- [ ] Performance benchmarking
- [ ] Update documentation
- [ ] Remove C# 11 workarounds
- [ ] Team training on C# 12 features

### Rollback Plan

If critical issues discovered:

1. Revert `<LangVersion>` to 11
2. Keep .NET 9 SDK (backward compatible)
3. Schedule additional time for investigation
4. Retry migration after fixes

## Success Criteria

- ✅ All tests pass (100%)
- ✅ No performance regression (±5%)
- ✅ Build time unchanged or improved
- ✅ Team confident with new features
- ✅ Documentation updated

## Risks and Mitigation

| Risk                             | Likelihood | Impact | Mitigation                       |
| -------------------------------- | ---------- | ------ | -------------------------------- |
| Breaking changes in dependencies | Medium     | High   | Test thoroughly in dev           |
| Performance regression           | Low        | High   | Benchmark before/after           |
| Team unfamiliar with features    | Medium     | Medium | Training sessions                |
| Extended timeline                | Medium     | Low    | Phase rollout allows flexibility |

````

### Example C: CI/CD Quality Gates

**Comprehensive GitHub Actions Workflow**:

```yaml
name: Evergreen Quality Gates

on:
  pull_request:
  push:
    branches: [main]

env:
  DOTNET_VERSION: '9.0.x'

jobs:
  # Job 1: Security Scanning
  security-scan:
    name: Security Scan
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
        with:
          fetch-depth: 0  # Full history for secret scanning

      # Secret Detection
      - name: TruffleHog Secret Scan
        uses: trufflesecurity/trufflehog@main
        with:
          path: ./
          base: ${{ github.event.pull_request.base.sha || 'main' }}
          head: HEAD
          extra_args: --only-verified --fail

      # Dependency Vulnerabilities
      - name: Setup .NET
        uses: actions/setup-dotnet@v4
        with:
          dotnet-version: ${{ env.DOTNET_VERSION }}

      - name: Restore Dependencies
        run: dotnet restore

      - name: Dependency Vulnerability Check
        run: dotnet list package --vulnerable --include-transitive
        continue-on-error: false

      # SBOM Generation
      - name: Generate SBOM
        run: |
          dotnet tool install --global CycloneDX
          dotnet CycloneDX . -o sbom.json

      - name: Upload SBOM
        uses: actions/upload-artifact@v3
        with:
          name: sbom
          path: sbom.json
          retention-days: 90

  # Job 2: Code Quality
  code-quality:
    name: Code Quality
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4

      - name: Setup .NET
        uses: actions/setup-dotnet@v4
        with:
          dotnet-version: ${{ env.DOTNET_VERSION }}

      # Code Formatting
      - name: Check Code Formatting
        run: dotnet format --verify-no-changes

      # Linting
      - name: Run Linters
        run: dotnet build /p:TreatWarningsAsErrors=true

      # Static Analysis
      - name: Run Roslyn Analyzers
        run: dotnet build /p:EnforceCodeStyleInBuild=true

  # Job 3: Tests and Coverage
  test-coverage:
    name: Tests and Coverage
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4

      - name: Setup .NET
        uses: actions/setup-dotnet@v4
        with:
          dotnet-version: ${{ env.DOTNET_VERSION }}

      - name: Restore Dependencies
        run: dotnet restore

      # Unit Tests
      - name: Run Unit Tests
        run: |
          dotnet test \
            --no-restore \
            --verbosity normal \
            /p:CollectCoverage=true \
            /p:CoverletOutputFormat=opencover \
            --filter "Category=Unit"

      # Integration Tests
      - name: Run Integration Tests
        run: |
          dotnet test \
            --no-restore \
            --verbosity normal \
            --filter "Category=Integration"

      # Coverage Check
      - name: Code Coverage Gate
        run: |
          dotnet tool install -g dotnet-coverage
          dotnet-coverage collect "dotnet test" -f xml -o coverage.xml

          # Parse coverage percentage
          COVERAGE=$(grep -oP 'line-rate="\K[^"]+' coverage.xml | \
            awk '{s+=$1} END {print s/NR*100}')

          echo "Code Coverage: $COVERAGE%"

          if (( $(echo "$COVERAGE < 80" | bc -l) )); then
            echo "❌ Coverage below 80% threshold"
            exit 1
          fi
          echo "✅ Coverage meets threshold"

      - name: Upload Coverage Report
        uses: codecov/codecov-action@v3
        with:
          files: ./coverage.xml
          fail_ci_if_error: true

  # Job 4: Build Verification
  build:
    name: Build Application
    runs-on: ubuntu-latest
    strategy:
      matrix:
        configuration: [Debug, Release]
    steps:
      - uses: actions/checkout@v4

      - name: Setup .NET
        uses: actions/setup-dotnet@v4
        with:
          dotnet-version: ${{ env.DOTNET_VERSION }}

      - name: Restore Dependencies
        run: dotnet restore

      - name: Build
        run: dotnet build --configuration ${{ matrix.configuration }} --no-restore

      - name: Publish
        run: dotnet publish --configuration ${{ matrix.configuration }} --no-build -o ./publish

      - name: Upload Build Artifact
        uses: actions/upload-artifact@v3
        with:
          name: app-${{ matrix.configuration }}
          path: ./publish

  # Job 5: Performance Baseline
  performance:
    name: Performance Baseline
    runs-on: ubuntu-latest
    if: github.event_name == 'pull_request'
    steps:
      - uses: actions/checkout@v4
        with:
          ref: ${{ github.base_ref }}

      - name: Setup .NET
        uses: actions/setup-dotnet@v4
        with:
          dotnet-version: ${{ env.DOTNET_VERSION }}

      - name: Run Baseline Benchmark
        run: |
          dotnet run --project benchmarks/AppBenchmarks.csproj \
            -c Release -- --exporters json --memory
          mv BenchmarkDotNet.Artifacts/results.json baseline.json

      - uses: actions/checkout@v4

      - name: Run Current Benchmark
        run: |
          dotnet run --project benchmarks/AppBenchmarks.csproj \
            -c Release -- --exporters json --memory
          mv BenchmarkDotNet.Artifacts/results.json current.json

      - name: Compare Performance
        run: |
          python scripts/compare-benchmarks.py \
            --baseline baseline.json \
            --current current.json \
            --threshold 10  # Fail if >10% regression

  # Final Gate: All Checks Must Pass
  all-gates-passed:
    name: All Quality Gates Passed
    needs: [security-scan, code-quality, test-coverage, build, performance]
    runs-on: ubuntu-latest
    if: always()
    steps:
      - name: Check Gate Status
        run: |
          if [ "${{ needs.security-scan.result }}" != "success" ] || \
             [ "${{ needs.code-quality.result }}" != "success" ] || \
             [ "${{ needs.test-coverage.result }}" != "success" ] || \
             [ "${{ needs.build.result }}" != "success" ] || \
             [ "${{ needs.performance.result }}" != "success" ]; then
            echo "❌ One or more quality gates failed"
            exit 1
          fi
          echo "✅ All quality gates passed"

      - name: Update Status
        uses: actions/github-script@v6
        with:
          script: |
            github.rest.repos.createCommitStatus({
              owner: context.repo.owner,
              repo: context.repo.repo,
              sha: context.sha,
              state: 'success',
              context: 'Evergreen Quality Gates',
              description: 'All gates passed'
            })
````

### Example D: Technical Debt Tracking

**Debt Ticket Template**:

````markdown
---
title: "[DEBT] Replace deprecated Authentication API"
labels: technical-debt, security, priority-high
assignees: backend-team
---

## Debt Description

Currently using deprecated `AuthService.ValidateUser()` method
which is scheduled for removal in Q3 2026.

## Impact

- **Frequency**: Used in 23 files across authentication flows
- **Business Impact**: High - affects all user authentication
- **Developer Impact**: Medium - adds complexity to new auth features
- **Risk**: High - API removal will break authentication

## Current State

```csharp
// Deprecated method
var isValid = AuthService.ValidateUser(username, password);
```
````

## Desired State

```csharp
// Modern approach with result pattern
var result = await _authService.AuthenticateAsync(
    new AuthRequest(username, password));

if (result.IsSuccess) {
    // Handle success
}
```

## Effort Estimate

- **Complexity**: Medium
- **Estimated Hours**: 16 hours
- **Risk Level**: Medium (well-tested replacement available)

## Migration Plan

1. Week 1: Update authentication service layer (8 hours)
2. Week 2: Update controllers and middleware (6 hours)
3. Week 3: Update tests and documentation (2 hours)

## Priority Calculation

```
Score = (Impact: 8 × Frequency: 9) / (Effort: 6 × Risk: 5) = 2.4
Priority: High (score > 2.0)
```

## Related

- Migration Guide: docs/auth-migration.md
- New API Docs: docs/api/authentication-v2.md
- Deprecation Notice: https://github.com/company/auth-lib/issues/456

## Acceptance Criteria

- [ ] All 23 files migrated to new API
- [ ] Tests updated and passing
- [ ] No references to deprecated method remain
- [ ] Documentation updated
- [ ] Team trained on new patterns

````

**Debt Dashboard (Markdown Table)**:
```markdown
# Technical Debt Dashboard - February 2026

## Summary Metrics
- **Total Debt Items**: 47
- **Critical Priority**: 3
- **High Priority**: 12
- **Medium Priority**: 18
- **Low Priority**: 14
- **Average Age**: 45 days
- **Debt Trend**: ↓ Decreasing (8 resolved last month)

## Critical Items (Immediate Action Required)

| ID | Title | Age | Impact | Effort | Owner | Due Date |
|----|-------|-----|--------|--------|-------|----------|
| DEBT-001 | Security vulnerability in auth lib | 12d | Critical | 2d | @alice | 2026-02-12 |
| DEBT-002 | Memory leak in background job | 8d | High | 3d | @bob | 2026-02-15 |
| DEBT-003 | Deprecated API blocking new features | 34d | High | 5d | @charlie | 2026-02-20 |

## Module Hotspots (Areas with Most Debt)

| Module | Debt Items | Total Effort (hours) | Priority Score |
|--------|-----------|---------------------|----------------|
| Authentication | 8 | 64 | 3.2 |
| Payment Processing | 6 | 48 | 2.8 |
| Reporting Engine | 5 | 40 | 2.1 |
| User Management | 4 | 24 | 1.5 |

## Trends

### Monthly Resolution Rate
````

Jan: ████████░░ 12 resolved / 15 created (0.8 ratio)
Feb: ██████████ 14 resolved / 8 created (1.75 ratio) ← Improving!

```

### Debt Age Distribution
- 0-30 days: 18 items (38%)
- 31-60 days: 15 items (32%)
- 61-90 days: 10 items (21%)
- 90+ days: 4 items (9%) ← Target for next sprint

## Action Items
1. **URGENT**: Address 3 critical items this sprint
2. Focus on Authentication module (highest hotspot)
3. Tackle 4 items over 90 days old
4. Maintain >1.0 resolution ratio (resolving more than creating)
```

## Tool Recommendations

### Dependency Management

- **Renovate** (Recommended): Advanced automation, flexible configuration
- **Dependabot**: Built into GitHub, simpler setup
- **Snyk**: Security-focused with excellent vulnerability database
- **WhiteSource/Mend**: Enterprise license compliance + security

### Security Scanning

- **Snyk**: Best for JavaScript/Python/Ruby ecosystems
- **OWASP Dependency-Check**: Open-source, multi-language
- **TruffleHog**: Excellent secret detection in git history
- **GitGuardian**: Real-time secret monitoring
- **Semgrep**: Custom security rules + community patterns

### Code Quality

- **SonarQube** (Recommended): Comprehensive code quality platform
- **CodeClimate**: Developer-friendly quality metrics
- **Roslyn Analyzers**: Built-in for C#/.NET
- **ESLint**: JavaScript/TypeScript standard
- **Pylint/Black**: Python code quality

### SBOM Generation

- **CycloneDX** (Recommended): Purpose-built for security use cases
- **SPDX**: Standard for detailed software provenance
- **Syft**: Fast multi-ecosystem SBOM generator

### Performance Monitoring

- **Application Insights**: Azure-native APM
- **Datadog**: Comprehensive observability platform
- **New Relic**: APM with strong AI/ML features
- **Prometheus + Grafana**: Open-source monitoring stack

### Documentation

- **Swagger/OpenAPI**: API documentation standard
- **DocFX**: .NET documentation generator
- **JSDoc**: JavaScript documentation
- **Sphinx**: Python documentation
- **MkDocs**: Markdown-based documentation

## Metrics and KPIs

### Key Performance Indicators

**Dependency Health**:

```
Dependency Freshness = (Current Dependencies / Total Dependencies) × 100
Target: >85% of dependencies within 6 months of latest version

Example Calculation:
- Total Dependencies: 120
- Current (within 6 months): 105
- Outdated (>6 months): 15
- Freshness: 87.5% ✅
```

**Security Posture**:

```
Security Score = 100 - (Critical×10 + High×5 + Medium×2 + Low×0.5)
Target: >90

Example Calculation:
- Critical: 0 → 0 points
- High: 2 → 10 points
- Medium: 5 → 10 points
- Low: 8 → 4 points
- Score: 100 - 24 = 76 ⚠️ (needs improvement)
```

**Technical Debt Ratio**:

```
Debt Ratio = (Debt Remediation Time / Total Development Time) × 100
Target: <5%

Example Calculation:
- Development Hours (last sprint): 320 hours
- Debt Remediation: 12 hours
- Ratio: (12 / 320) × 100 = 3.75% ✅
```

**Test Coverage**:

```
Code Coverage = (Covered Lines / Total Executable Lines) × 100
Target: >80%

Branch Coverage = (Covered Branches / Total Branches) × 100
Target: >75%
```

**Code Quality Score**:

```
Quality Score = (
  (100 - Complexity_Score) × 0.3 +
  Coverage_Score × 0.3 +
  Maintainability_Rating × 0.2 +
  (100 - Duplication_%) × 0.2
)

Target: >85
```

**Mean Time to Patch (MTTP)**:

```
MTTP = Average(Patch_Applied_Date - Vulnerability_Disclosed_Date)

Targets by Severity:
- Critical: <24 hours
- High: <7 days
- Medium: <30 days
```

**Build Performance**:

```
Build Efficiency = Successful Builds / Total Builds × 100
Target: >95%

Average Build Duration Trend: Should not increase >10% per quarter
```

## Review Cycles

### Daily Reviews

- **Security Vulnerability Scans**: Automated via CI/CD
- **Build Health Check**: Dashboard review
- **Critical Alert Response**: On-call monitoring

### Weekly Reviews

- **Dependency Update Review**: Team reviews Dependabot/Renovate PRs
- **Code Quality Assessment**: SonarQube report review
- **Test Coverage Analysis**: Identify coverage gaps
- **Incident Review**: Post-mortem for any production issues

**Weekly Review Template**:

```markdown
# Weekly Evergreen Review - Week of YYYY-MM-DD

## Attendees

- Tech Lead
- 2-3 Engineers (rotating)
- DevOps Representative

## Agenda (30 minutes)

### 1. Security (5 min)

- New vulnerabilities discovered: X
- Vulnerabilities patched: Y
- Overdue security items: Z

### 2. Dependencies (10 min)

- Pending update PRs: Review and approve/reject
- Blocked updates: Discuss mitigation
- Dependency freshness trend: ↑↓

### 3. Code Quality (10 min)

- Quality score trend: X → Y
- New code smells: Z
- Technical debt resolved: A items

### 4. Testing (5 min)

- Coverage: X% (Δ +/- Y%)
- Flaky tests: Z tests to fix
- Test suite performance: X minutes

### Action Items

- [ ] @person: Task 1 (due: date)
- [ ] @person: Task 2 (due: date)
```

### Monthly Reviews

- **Architecture Review**: Technology stack evaluation
- **Technical Debt Sprint Planning**: Prioritize debt backlog
- **Performance Baseline**: Trend analysis and optimization needs
- **Team Retrospective**: Process improvements

### Quarterly Reviews

- **Framework Evaluation**: Assess major framework versions
- **Tool Assessment**: Evaluate CI/CD and quality tools
- **Language Version Planning**: Plan major language upgrades
- **Infrastructure Review**: Cloud service and container updates

### Annual Reviews

- **Technology Stack Evaluation**: Consider major technology shifts
- **Process Audit**: Review evergreen process effectiveness
- **Training Needs**: Identify skill gaps and training plans
- **Long-term Roadmap**: Multi-year modernization strategy

## Roles and Responsibilities

### Developers

**Daily**:

- Review and address linting warnings
- Write tests for new code
- Address code review feedback
- Monitor build status

**Weekly**:

- Review dependency update PRs
- Participate in code quality reviews
- Address assigned technical debt items
- Update documentation for changes

**Monthly**:

- Contribute to technical debt prioritization
- Participate in retrospectives

### Tech Leads

**Daily**:

- Review critical security alerts
- Approve high-priority PRs
- Mentor team on best practices

**Weekly**:

- Lead evergreen reviews
- Approve major dependency updates
- Review technical debt trends
- Monitor team capacity allocation

**Monthly**:

- Conduct architecture reviews
- Plan technical debt sprints
- Report metrics to management

**Quarterly**:

- Evaluate frameworks and tools
- Plan major migrations
- Update development standards

### DevOps Engineers

**Daily**:

- Monitor CI/CD pipeline health
- Respond to build failures
- Update security scan configurations

**Weekly**:

- Review build performance metrics
- Update CI/CD tools and actions
- Optimize build times

**Monthly**:

- Update infrastructure components
- Review monitoring and alerting
- Optimize deployment pipelines

**Quarterly**:

- Evaluate CI/CD platform features
- Plan infrastructure upgrades

### Security Team

**Daily**:

- Triage new vulnerability alerts
- Respond to critical security issues

**Weekly**:

- Review security scan results
- Validate SBOM accuracy
- Approve security exception requests

**Monthly**:

- Conduct security audits
- Update security policies
- Train developers on threats

**Quarterly**:

- Penetration testing
- Compliance reviews
- Security roadmap updates

### QA Team

**Daily**:

- Monitor test suite health
- Investigate test failures

**Weekly**:

- Review test coverage reports
- Update test data and fixtures
- Identify flaky tests

**Monthly**:

- Audit test suite effectiveness
- Performance test baseline updates
- Test environment maintenance

## Non-Compliance and Remediation

### Warning Thresholds

**Dependency Freshness**:

- ⚠️ **Warning**: >30 days behind on minor/patch updates
- 🚨 **Critical**: >90 days behind on any updates

**Security Vulnerabilities**:

- ⚠️ **Warning**: Any high severity vulnerabilities
- 🚨 **Critical**: Any critical severity vulnerabilities
- 🛑 **Blocking**: Critical vulnerabilities >24 hours old

**Code Coverage**:

- ⚠️ **Warning**: Coverage <75%
- 🚨 **Critical**: Coverage <70%
- 🛑 **Blocking**: Coverage decrease >5% in single PR

**Technical Debt**:

- ⚠️ **Warning**: Debt ratio >5%
- 🚨 **Critical**: Debt ratio >10%
- 🛑 **Blocking**: Critical priority items unaddressed >30 days

### Remediation Processes

**Dependency Non-Compliance**:

```markdown
## Remediation Steps

1. **Immediate (Day 1)**
   - Identify all outdated dependencies
   - Categorize by risk (security, breaking changes, effort)
   - Create tracking ticket for each major update

2. **Short-term (Week 1)**
   - Update low-risk dependencies (patch versions)
   - Test thoroughly in staging
   - Deploy to production

3. **Medium-term (Month 1)**
   - Create migration plans for breaking changes
   - Schedule updates in sprint planning
   - Allocate 25% sprint capacity to updates

4. **Documentation**
   - Record why updates were delayed
   - Document migration challenges
   - Update dependency update policy if needed
```

**Security Non-Compliance**:

```markdown
## Security Remediation (Critical Path)

**Hour 0-4: Assessment**

- Confirm vulnerability applicability
- Assess exploit likelihood
- Determine business impact

**Hour 4-8: Mitigation**

- Implement temporary mitigations (firewall rules, feature flags)
- Prepare patch or update
- Test in isolated environment

**Hour 8-24: Deployment**

- Deploy patch to staging
- Smoke test critical paths
- Deploy to production with monitoring
- Validate exploitation is blocked

**Hour 24-48: Verification**

- Monitor for anomalies
- Verify vulnerability scans pass
- Update security documentation

**Post-Incident**

- Write incident report
- Conduct blameless postmortem
- Update security processes
- Train team on prevention
```

### Escalation Paths

**Level 1: Developer + Tech Lead**

- Minor quality issues
- Non-security dependency updates
- Code smell remediation

**Level 2: Manager + Architecture Team**

- High severity security vulnerabilities
- Major version migrations
- Persistent technical debt

**Level 3: Director + Executive Team**

- Critical security breaches
- Compliance violations
- Strategic technology decisions

## Integration with Existing Policies

### Cross-References

This instruction file integrates with:

- **[AI-Assisted Output Policy](.github/instructions/ai-assisted-output.instructions.md)**: All AI-generated modernization artifacts must include full provenance metadata
- **Code Review Guidelines**: Evergreen checks are part of PR review process
- **Security Policy**: Vulnerability management aligns with security requirements
- **Testing Standards**: Coverage and quality thresholds enforce testing policy

### Compliance Alignment

**Security Compliance**:

- SBOM requirements align with NIST SSDF
- Vulnerability management meets SOC 2 requirements
- Secret detection supports PCI DSS compliance

**Quality Compliance**:

- Code coverage supports ISO 25010 quality standards
- Technical debt tracking aligns with SQALE methodology
- Documentation requirements meet ISO 9001 standards

### Policy Conflicts

If conflicts arise between evergreen requirements and other policies:

1. **Security Always Wins**: Security requirements override all others
2. **Document Exceptions**: Record why standard process not followed
3. **Escalate Uncertainty**: When unclear, escalate to tech leadership
4. **Update Policies**: If conflicts recur, update policies for clarity

## Conclusion

Evergreen software development is not a one-time effort—it's a continuous commitment to software health, security, and maintainability. By following these practices, teams build sustainable codebases that support long-term business value without accumulating crippling technical debt.

**Key Takeaways**:

1. **Allocate Capacity**: Reserve 15-25% of each sprint for evergreen activities
2. **Automate Ruthlessly**: Use tools to detect and prevent issues early
3. **Measure Progress**: Track metrics and trends, not just absolute values
4. **Foster Culture**: Celebrate maintenance work as much as new features
5. **Stay Disciplined**: Consistent small efforts beat sporadic large cleanups

**Getting Started**:

1. **Week 1**: Set up automated dependency updates and security scanning
2. **Week 2**: Establish code quality gates in CI/CD
3. **Week 3**: Create technical debt backlog and prioritize top 10 items
4. **Week 4**: Hold first weekly evergreen review meeting
5. **Month 2+**: Refine processes based on team feedback and metrics

**Remember**: Evergreen software development is a journey, not a destination. Start small, build momentum, and continuously improve.

---

**Document Version**: 1.0.0
**Last Updated**: 2026-02-10
**Next Review**: 2026-05-10 (Quarterly)
**Feedback**: Submit improvements via PR or issue
