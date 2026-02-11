---
mode: agent
model: "anthropic/claude-3.5-sonnet@2024-10-22"
tools: ["create", "search", "read"]
description: Generates comprehensive instruction file for evergreen software development practices
prompt_metadata:
  id: create-evergreen-development-instructions
  title: Evergreen Software Development Instructions Generator
  owner: johnmillerATcodemag-com
  version: 1.0.0
  created: 2026-02-10
  updated: 2026-02-10
  output_path: .github/instructions/evergreen-development.instructions.md
  output_format: markdown
  category: software-development
  tags:
    [
      evergreen,
      continuous-improvement,
      technical-debt,
      modernization,
      sustainable-development,
      maintenance,
      best-practices,
    ]
---

# Generate Evergreen Software Development Instructions

## Context

You are tasked with creating a comprehensive instruction file that defines evergreen software development practices for this repository. Evergreen software development refers to the practice of continuously maintaining, updating, and modernizing software to prevent technical debt accumulation and ensure long-term sustainability, security, and relevance.

**CRITICAL**: All AI-generated artifacts MUST comply with `.github/instructions/ai-assisted-output.instructions.md`. The generated instruction file MUST include full AI provenance metadata.

## Objective

Create `.github/instructions/evergreen-development.instructions.md` that provides:

1. Clear definition and principles of evergreen software development
2. Actionable practices for continuous modernization
3. Dependency management and update strategies
4. Code quality and technical debt management
5. Security and vulnerability management
6. Documentation maintenance requirements
7. Testing and quality assurance evolution
8. Technology stack evaluation and migration paths
9. Automated tooling and CI/CD integration
10. Compliance checks and enforcement mechanisms

## Target Audience

- Software developers maintaining codebases
- DevOps engineers managing CI/CD pipelines
- Technical leads making architectural decisions
- AI assistants generating or modifying code
- Quality assurance teams maintaining test suites

## Key Principles to Address

### 1. Continuous Dependency Updates

- Regular package/library updates
- Security patch application
- Breaking change management
- Compatibility testing strategies

### 2. Code Modernization

- Language version upgrades
- Framework updates
- API migration paths
- Deprecation handling
- Pattern modernization

### 3. Technical Debt Prevention

- Code review standards
- Refactoring cadence
- Complexity metrics
- Dead code elimination
- Documentation currency

### 4. Security Posture

- Vulnerability scanning automation
- SBOM (Software Bill of Materials) maintenance
- Secret detection
- Dependency audit trails
- Security patch SLAs

### 5. Quality Standards

- Test coverage maintenance
- Performance benchmarking
- Code quality metrics
- Static analysis integration
- Linting and formatting

### 6. Infrastructure Evolution

- Build system updates
- CI/CD pipeline modernization
- Container base image updates
- Cloud service upgrades
- Tooling version management

## Deliverable

Generate `.github/instructions/evergreen-development.instructions.md` with:

### Required AI Provenance Metadata (YAML Front Matter)

```yaml
---
ai_generated: true
model: "<provider>/<model-name>@<version>"
operator: "<github-username>"
chat_id: "<unique-chat-identifier>"
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
started: "<ISO8601-timestamp>"
ended: "<ISO8601-timestamp>"
task_durations:
  - task: "requirements analysis"
    duration: "<hh:mm:ss>"
  - task: "content generation"
    duration: "<hh:mm:ss>"
  - task: "examples and templates"
    duration: "<hh:mm:ss>"
total_duration: "<hh:mm:ss>"
ai_log: "ai-logs/<yyyy>/<mm>/<dd>/<chat-id>/conversation.md"
source: ".github/prompts/create-evergreen-development-instructions.prompt.md"
applyTo: "**"
---
```

### Content Structure Requirements

The instruction file must include:

#### 1. Overview Section

- Definition of evergreen software development
- Benefits and business value
- Relationship to technical debt
- Scope and applicability

#### 2. Core Principles

- Continuous improvement mindset
- Proactive vs reactive maintenance
- Sustainable development velocity
- Balance between innovation and stability

#### 3. Dependency Management

- **Update Frequency**: Define cadence for different dependency types
- **Breaking Change Strategy**: How to handle major version updates
- **Security Patches**: SLA for critical security updates
- **Compatibility Testing**: Required validation before updates
- **Dependency Pinning vs Ranges**: When to use each approach
- **Tool Integration**: Dependabot, Renovate, or similar tools

#### 4. Code Modernization Practices

- **Language Version Policy**: When and how to upgrade language versions
- **Framework Updates**: Strategy for major framework migrations
- **API Deprecation Handling**: Timeline and communication
- **Pattern Updates**: Refactoring to modern idioms
- **Code Quality Standards**: Linting, formatting, complexity limits

#### 5. Technical Debt Management

- **Debt Identification**: Methods for finding technical debt
- **Prioritization Framework**: How to prioritize debt remediation
- **Refactoring Guidelines**: When and how to refactor
- **Code Review Standards**: Preventing new debt
- **Metrics and Tracking**: Measuring debt accumulation

#### 6. Security and Vulnerability Management

- **Vulnerability Scanning**: Tools and frequency
- **SBOM Requirements**: Generation and maintenance
- **Secret Detection**: Automated scanning and remediation
- **Security Update SLAs**: Response times by severity
- **Audit Trails**: Tracking security-related changes

#### 7. Testing and Quality Assurance

- **Test Evolution**: Keeping tests current with code
- **Coverage Requirements**: Minimum coverage thresholds
- **Performance Testing**: Regression detection
- **Integration Testing**: External dependency validation
- **Test Maintenance**: Removing obsolete tests

#### 8. Documentation Currency

- **API Documentation**: Auto-generation and validation
- **README Maintenance**: Regular review cycles
- **Code Comments**: When to update or remove
- **Architecture Docs**: Keeping diagrams current
- **Change Documentation**: Recording significant updates

#### 9. CI/CD and Automation

- **Pipeline Modernization**: Regular pipeline reviews
- **Tool Upgrades**: CI/CD platform and tool updates
- **Build Optimization**: Performance improvements
- **Automated Checks**: Quality gates and blockers
- **Deployment Strategies**: Blue-green, canary, feature flags

#### 10. Infrastructure Evolution

- **Container Base Images**: Update policies
- **Cloud Service Versions**: Upgrade strategies
- **Database Migrations**: Backward compatibility
- **Configuration Management**: IaC updates
- **Monitoring and Observability**: Telemetry evolution

#### 11. Compliance and Enforcement

- **PR Requirements**: Mandatory checks before merge
- **CI Gates**: Automated blocking conditions
- **Audit Reports**: Regular compliance reviews
- **Exception Process**: How to handle special cases
- **Remediation Deadlines**: Timelines for fixing issues

#### 12. Practical Examples

Include concrete examples:

**Example A: Dependency Update Workflow**

```yaml
# Sample Dependabot or Renovate configuration
# Sample PR checklist for dependency updates
# Sample testing requirements before approval
```

**Example B: Code Modernization Template**

```markdown
# Migration plan template for major updates

# Before/after code examples

# Rollback procedures
```

**Example C: CI/CD Quality Gates**

```yaml
# GitHub Actions workflow example
# Azure DevOps pipeline example
# Security scanning integration
# SBOM generation step
```

**Example D: Technical Debt Tracking**

```markdown
# Debt identification checklist

# Prioritization matrix

# Remediation plan template
```

#### 13. Tool Recommendations

Provide specific tool suggestions for:

- Dependency management (Dependabot, Renovate, etc.)
- Security scanning (Snyk, OWASP Dependency Check, TruffleHog)
- Code quality (SonarQube, CodeClimate, ESLint, Roslyn)
- SBOM generation (CycloneDX, SPDX)
- Performance monitoring (Application Insights, Prometheus)
- Documentation (Swagger/OpenAPI, DocFX, JSDoc)

#### 14. Metrics and KPIs

Define measurable indicators:

- Dependency freshness (average age of dependencies)
- Security vulnerability count by severity
- Technical debt ratio (time to fix vs time to build)
- Test coverage percentage
- Code quality scores
- Documentation completeness
- Mean time to patch (MTTP) for security issues
- Build and deployment frequency

#### 15. Review Cycles

Establish review cadences:

- **Daily**: Security vulnerability scans
- **Weekly**: Dependency update reviews
- **Monthly**: Code quality assessments, test coverage analysis
- **Quarterly**: Architecture reviews, framework evaluation
- **Annually**: Technology stack evaluation, major migrations

#### 16. Roles and Responsibilities

Define ownership:

- **Developers**: Day-to-day code quality, PR reviews
- **Tech Leads**: Architecture decisions, migration planning
- **DevOps**: CI/CD maintenance, security scanning
- **Security Team**: Vulnerability triage, security policy
- **QA**: Test evolution, coverage validation

#### 17. Non-Compliance and Remediation

Address enforcement:

- Warning thresholds
- Blocking conditions
- Remediation processes
- Escalation paths
- Documentation requirements

#### 18. Integration with Existing Policies

Cross-reference:

- Link to AI-assisted output policy (ai-assisted-output.instructions.md)
- Link to code review guidelines
- Link to security policies
- Link to testing standards

## Quality Requirements

The generated instruction file must:

- ✅ Be optimized for AI agent consumption and processing
- ✅ Minimize token usage while maintaining completeness
- ✅ Use clear, hierarchical structure with consistent heading levels
- ✅ Include actionable, specific guidance (not vague principles)
- ✅ Provide concrete examples and templates
- ✅ Define measurable criteria where possible
- ✅ Include enforcement mechanisms and compliance checks
- ✅ Cross-reference related instruction files
- ✅ Balance comprehensiveness with brevity
- ✅ Use tables, lists, and code blocks for clarity

## Success Criteria

The instruction file succeeds when:

1. Developers can immediately apply practices from reading it
2. AI assistants can enforce policies automatically using it
3. CI/CD pipelines can implement gates based on it
4. Technical debt is measurably reduced over time
5. Security posture measurably improves
6. Dependency freshness increases
7. Code quality metrics trend positively
8. Development velocity remains sustainable

## Output Format

- **Format**: Markdown with YAML front matter
- **Path**: `.github/instructions/evergreen-development.instructions.md`
- **Style**: Direct, imperative, technical
- **Target**: Developers and AI assistants
- **Length**: Comprehensive but concise (aim for 1500-2500 lines)

## Post-Generation Steps

After generating the instruction file:

1. Create conversation log: `ai-logs/<yyyy>/<mm>/<dd>/<chat-id>/conversation.md`
2. Create summary: `ai-logs/<yyyy>/<mm>/<dd>/<chat-id>/summary.md`
3. Update main README.md with entry:
   ```markdown
   - [Evergreen Development Instructions](.github/instructions/evergreen-development.instructions.md) -
     Comprehensive practices for continuous software modernization and technical debt prevention
     ([AI Log](ai-logs/<yyyy>/<mm>/<dd>/<chat-id>/conversation.md))
   ```
4. Validate all internal links
5. Verify YAML front matter completeness
6. Run through quality checklist

## Notes

- Focus on **actionable practices**, not abstract theory
- Emphasize **automation** where possible
- Provide **specific tool recommendations** with configuration examples
- Include **enforcement mechanisms** (CI gates, PR checks)
- Address **common pitfalls** and how to avoid them
- Balance **rigor with pragmatism** (not every project needs every practice)
- Consider **different project scales** (startup vs enterprise)
- Ensure **compatibility with existing instruction files** in this repository
