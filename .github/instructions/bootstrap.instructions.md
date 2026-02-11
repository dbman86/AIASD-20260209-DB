---
ai_generated: true
model: "anthropic/claude-3.5-sonnet@2024-10-22"
operator: "assistant"
chat_id: "bootstrap-instructions-20260210"
prompt: |
  create an instruction file for these technologies
  UI Framework
  Bootstrap - CSS framework for responsive design
  Bootstrap JavaScript - Interactive components (bundle includes Popper.js)
started: "2026-02-10T20:00:00Z"
ended: "2026-02-10T20:45:00Z"
task_durations:
  - task: "requirements analysis"
    duration: "00:05:00"
  - task: "content generation"
    duration: "00:30:00"
  - task: "examples and validation"
    duration: "00:10:00"
total_duration: "00:45:00"
ai_log: "ai-logs/2026/02/10/bootstrap-instructions-20260210/conversation.md"
source: "user-request"
applyTo: "**/*.{cshtml,html,css,js}"
---

# Bootstrap CSS and JavaScript Standards and Practices

## Overview

This document establishes standards, best practices, and usage patterns for **Bootstrap 5.x** CSS framework and Bootstrap JavaScript components in web applications. These guidelines ensure consistent, accessible, responsive, and performant user interfaces that follow modern web development practices.

### Technology Stack

- **Bootstrap CSS**: Responsive, mobile-first CSS framework with utility classes and components
- **Bootstrap JavaScript**: Interactive component library built on vanilla JavaScript
- **Popper.js**: Positioning engine for tooltips, popovers, and dropdowns (bundled with Bootstrap JS)

### Scope

This instruction applies to:

- All HTML/Razor Pages using Bootstrap CSS classes
- Bootstrap JavaScript component initialization and configuration
- Custom styling and Bootstrap theme customization
- Responsive design implementation
- Accessibility considerations for Bootstrap components

**Related Documentation**:

- [Razor Pages and Kestrel Instructions](.github/instructions/razor-pages-kestrel.instructions.md)
- [Evergreen Development Instructions](.github/instructions/evergreen-development.instructions.md)

## Table of Contents

- [Bootstrap Version Policy](#bootstrap-version-policy)
- [Installation and Setup](#installation-and-setup)
- [CSS Framework Standards](#css-framework-standards)
- [Responsive Design Patterns](#responsive-design-patterns)
- [Bootstrap Components](#bootstrap-components)
- [JavaScript Component Management](#javascript-component-management)
- [Customization and Theming](#customization-and-theming)
- [Accessibility Guidelines](#accessibility-guidelines)
- [Performance Optimization](#performance-optimization)
- [Common Patterns and Examples](#common-patterns-and-examples)

## Bootstrap Version Policy

### Version Requirements

**Current Standard**: Bootstrap 5.3.x (latest stable)

**Version Update Policy**:

- **Major versions** (5.x → 6.x): Evaluate within 3 months, migrate within 6 months
- **Minor versions** (5.2 → 5.3): Update within 1 month
- **Patch versions** (5.3.0 → 5.3.1): Update within 1 week

**Deprecation Handling**:

- Monitor Bootstrap changelog for deprecation notices
- Plan migration before deprecated features are removed
- Document custom code using deprecated features

**Version Pinning**:

```json
// package.json - Pin to specific minor version
{
  "dependencies": {
    "bootstrap": "^5.3.0",
    "@popperjs/core": "^2.11.8"
  }
}
```

**CDN Version (for prototyping only)**:

```html
<!-- Bootstrap CSS -->
<link
  href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css"
  rel="stylesheet"
/>

<!-- Bootstrap Bundle JS (includes Popper.js) -->
<script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js"></script>
```

⚠️ **Warning**: Use CDN only for prototyping. Production apps should use local copies or bundled assets.

## Installation and Setup

### Recommended Installation Methods

**Method 1: npm/yarn (Recommended for ASP.NET Core)**

```bash
# Install Bootstrap and Popper.js
npm install bootstrap @popperjs/core

# Or using yarn
yarn add bootstrap @popperjs/core
```

**Method 2: LibMan (Visual Studio Library Manager)**

```bash
# Install via CLI
libman install bootstrap@5.3.2 --provider jsdelivr --destination wwwroot/lib/bootstrap

# Or use libman.json configuration
```

```json
// libman.json
{
  "version": "1.0",
  "defaultProvider": "jsdelivr",
  "libraries": [
    {
      "library": "bootstrap@5.3.2",
      "destination": "wwwroot/lib/bootstrap",
      "files": [
        "dist/css/bootstrap.min.css",
        "dist/css/bootstrap.min.css.map",
        "dist/js/bootstrap.bundle.min.js",
        "dist/js/bootstrap.bundle.min.js.map"
      ]
    }
  ]
}
```

**Method 3: NuGet Package (ASP.NET Core)**

```xml
<!-- For server-side only scenarios -->
<PackageReference Include="Bootstrap" Version="5.3.2" />
```

### Project Setup

**ASP.NET Core Razor Pages Layout**:

```cshtml
<!-- Pages/Shared/_Layout.cshtml -->
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>@ViewData["Title"] - My App</title>

    <!-- Bootstrap CSS -->
    <link rel="stylesheet" href="~/lib/bootstrap/dist/css/bootstrap.min.css" />

    <!-- Custom CSS (loaded after Bootstrap) -->
    <link rel="stylesheet" href="~/css/site.css" asp-append-version="true" />
</head>
<body>
    <header>
        <nav class="navbar navbar-expand-lg navbar-light bg-light">
            <!-- Navigation content -->
        </nav>
    </header>

    <main role="main" class="container">
        @RenderBody()
    </main>

    <footer class="footer mt-auto py-3 bg-light">
        <!-- Footer content -->
    </footer>

    <!-- Bootstrap Bundle JS (includes Popper.js) -->
    <script src="~/lib/bootstrap/dist/js/bootstrap.bundle.min.js"></script>

    <!-- Custom JavaScript -->
    <script src="~/js/site.js" asp-append-version="true"></script>

    @await RenderSectionAsync("Scripts", required: false)
</body>
</html>
```

**Critical Setup Rules**:

- ✅ Always load Bootstrap CSS before custom CSS
- ✅ Load Bootstrap JS at end of `<body>` for better page load performance
- ✅ Use `bootstrap.bundle.min.js` (includes Popper.js) for simplicity
- ✅ Include viewport meta tag for responsive design
- ❌ Don't load both `bootstrap.bundle.js` and `bootstrap.js` + `popper.js` separately
- ❌ Don't override Bootstrap variables inline in HTML

## CSS Framework Standards

### Utility-First Approach

**Preferred**: Use Bootstrap utility classes over custom CSS when possible

```html
<!-- ✅ Good: Bootstrap utilities -->
<div
  class="d-flex justify-content-between align-items-center mb-3 p-4 bg-light rounded"
>
  <h2 class="mb-0">Title</h2>
  <button class="btn btn-primary">Action</button>
</div>

<!-- ❌ Avoid: Custom CSS when utilities exist -->
<div class="custom-header">
  <h2>Title</h2>
  <button>Action</button>
</div>
<style>
  .custom-header {
    display: flex;
    justify-content: space-between;
    align-items: center;
    margin-bottom: 1rem;
    padding: 1.5rem;
    background-color: #f8f9fa;
    border-radius: 0.25rem;
  }
</style>
```

### Spacing System

Bootstrap uses a consistent spacing scale (0-5, plus auto):

```html
<!-- Margin: m-{side}-{size} -->
<div class="mt-3">
  <!-- margin-top: 1rem -->
  <div class="mb-4">
    <!-- margin-bottom: 1.5rem -->
    <div class="mx-auto">
      <!-- margin-left/right: auto (centering) -->
      <div class="my-5">
        <!-- margin-top/bottom: 3rem -->

        <!-- Padding: p-{side}-{size} -->
        <div class="pt-2">
          <!-- padding-top: 0.5rem -->
          <div class="px-4">
            <!-- padding-left/right: 1.5rem -->
            <div class="p-0">
              <!-- padding: 0 (remove all padding) -->

              <!-- Spacing scale -->
              0 = 0 1 = 0.25rem (4px) 2 = 0.5rem (8px) 3 = 1rem (16px) 4 =
              1.5rem (24px) 5 = 3rem (48px)
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</div>
```

**Spacing Guidelines**:

- ✅ Use consistent spacing values (don't use custom margins like `15px`)
- ✅ Prefer spacing utilities over custom CSS
- ✅ Use negative margins (`mt-n3`) sparingly and only when necessary
- ❌ Avoid mixing spacing systems (Bootstrap + custom pixel values)

### Typography

**Headings and Display**:

```html
<!-- Standard headings -->
<h1>Heading 1</h1>
<!-- 2.5rem -->
<h2>Heading 2</h2>
<!-- 2rem -->
<h3>Heading 3</h3>
<!-- 1.75rem -->

<!-- Display headings (larger) -->
<h1 class="display-1">Display 1</h1>
<!-- 5rem -->
<h1 class="display-4">Display 4</h1>
<!-- 3.5rem -->

<!-- Lead paragraph -->
<p class="lead">Make a paragraph stand out.</p>

<!-- Text utilities -->
<p class="text-muted">Muted text</p>
<p class="text-primary">Primary color text</p>
<p class="fw-bold">Bold text</p>
<p class="fst-italic">Italic text</p>
<p class="text-uppercase">Uppercase text</p>
```

**Typography Best Practices**:

- ✅ Use semantic HTML headings (`<h1>` - `<h6>`)
- ✅ Apply Bootstrap classes for styling (`display-*`, `lead`, etc.)
- ✅ Maintain heading hierarchy for accessibility
- ✅ Use `text-*` utilities for color and styling
- ❌ Don't skip heading levels (e.g., `<h1>` → `<h3>`)
- ❌ Don't use headings solely for sizing (use display/styling classes)

### Color System

**Semantic Colors**:

```html
<!-- Text colors -->
<p class="text-primary">Primary brand color</p>
<p class="text-secondary">Secondary color</p>
<p class="text-success">Success state</p>
<p class="text-danger">Error/danger state</p>
<p class="text-warning">Warning state</p>
<p class="text-info">Informational state</p>
<p class="text-light">Light text</p>
<p class="text-dark">Dark text</p>
<p class="text-muted">Muted text</p>

<!-- Background colors -->
<div class="bg-primary text-white">Primary background</div>
<div class="bg-light">Light background</div>
<div class="bg-transparent">Transparent background</div>

<!-- Opacity variations (Bootstrap 5.1+) -->
<div class="bg-primary bg-opacity-75">75% opacity</div>
<div class="text-primary text-opacity-50">50% opacity text</div>
```

**Color Usage Guidelines**:

- ✅ Use semantic color names (`primary`, `success`, `danger`)
- ✅ Ensure sufficient contrast for accessibility (WCAG AA minimum)
- ✅ Use `text-white` or `text-dark` for readability on colored backgrounds
- ✅ Customize theme colors via CSS variables or Sass
- ❌ Don't use color as the only way to convey information
- ❌ Avoid hard-coding hex colors when Bootstrap colors exist

## Responsive Design Patterns

### Breakpoint System

Bootstrap uses mobile-first breakpoints:

```scss
// Breakpoint values
$grid-breakpoints: (
  xs: 0,
  // Extra small devices (portrait phones)
  sm: 576px,
  // Small devices (landscape phones)
  md: 768px,
  // Medium devices (tablets)
  lg: 992px,
  // Large devices (desktops)
  xl: 1200px,
  // Extra large devices (large desktops)
  xxl: 1400px, // Extra extra large devices
);
```

**Responsive Utility Classes**:

```html
<!-- Display utilities -->
<div class="d-none d-md-block">
  <!-- Hidden on mobile, visible on tablets+ -->
  <div class="d-block d-lg-none">
    <!-- Visible until large screens -->
    <div class="d-sm-flex">
      <!-- Flex from small screens up -->

      <!-- Responsive spacing -->
      <div class="mt-3 mt-md-4 mt-lg-5">
        <!-- Increasing margin at breakpoints -->
        <div class="p-2 p-lg-4">
          <!-- More padding on larger screens -->

          <!-- Responsive text alignment -->
          <div class="text-center text-md-start">
            <!-- Center on mobile, left-align on tablets+ -->

            <!-- Responsive typography -->
            <h1 class="fs-3 fs-md-2 fs-lg-1">
              <!-- Smaller heading on mobile -->
            </h1>
          </div>
        </div>
      </div>
    </div>
  </div>
</div>
```

### Grid System

**12-Column Grid**:

```html
<!-- Basic grid -->
<div class="container">
  <div class="row">
    <div class="col-12 col-md-6 col-lg-4">
      <!-- Full width mobile, half on tablet, third on desktop -->
    </div>
    <div class="col-12 col-md-6 col-lg-4">
      <!-- Column 2 -->
    </div>
    <div class="col-12 col-md-12 col-lg-4">
      <!-- Column 3 -->
    </div>
  </div>
</div>

<!-- Equal-width columns -->
<div class="row">
  <div class="col">Column 1</div>
  <div class="col">Column 2</div>
  <div class="col">Column 3</div>
</div>

<!-- Grid with gutters -->
<div class="row g-3">
  <!-- 1rem gutter -->
  <div class="col-md-6">...</div>
  <div class="col-md-6">...</div>
</div>

<!-- No gutters -->
<div class="row g-0">
  <div class="col">...</div>
</div>
```

**Grid Best Practices**:

- ✅ Always use `.container` or `.container-fluid` as the grid wrapper
- ✅ Use `.row` for horizontal groups of columns
- ✅ Columns must be direct children of `.row`
- ✅ Use responsive column classes (`col-{breakpoint}-{size}`)
- ✅ Leverage gutter utilities (`g-*`, `gx-*`, `gy-*`)
- ❌ Don't nest `.container` inside `.container`
- ❌ Don't skip the `.row` wrapper for columns
- ❌ Don't exceed 12 columns in a single row (creates new row)

### Container Types

```html
<!-- Fixed-width container (responsive breakpoints) -->
<div class="container">
  <!-- Max width at each breakpoint -->
</div>

<!-- Fluid container (100% width) -->
<div class="container-fluid">
  <!-- Always 100% width -->
</div>

<!-- Breakpoint containers (Bootstrap 5.2+) -->
<div class="container-sm">
  <!-- 100% until sm breakpoint -->
  <div class="container-md">
    <!-- 100% until md breakpoint -->
    <div class="container-lg">
      <!-- 100% until lg breakpoint -->
      <div class="container-xl">
        <!-- 100% until xl breakpoint -->
        <div class="container-xxl"><!-- 100% until xxl breakpoint --></div>
      </div>
    </div>
  </div>
</div>
```

**Container Selection**:

- Use `.container` for standard page layouts with max-width
- Use `.container-fluid` for full-width layouts (dashboards, admin panels)
- Use breakpoint containers for fluid-until-breakpoint layouts

## Bootstrap Components

### Navigation Components

**Navbar (Recommended Pattern)**:

```html
<nav class="navbar navbar-expand-lg navbar-light bg-light">
  <div class="container-fluid">
    <!-- Brand -->
    <a class="navbar-brand" href="/">
      <img
        src="/logo.png"
        alt="Logo"
        width="30"
        height="30"
        class="d-inline-block align-text-top"
      />
      My App
    </a>

    <!-- Mobile toggle button -->
    <button
      class="navbar-toggler"
      type="button"
      data-bs-toggle="collapse"
      data-bs-target="#navbarNav"
      aria-controls="navbarNav"
      aria-expanded="false"
      aria-label="Toggle navigation"
    >
      <span class="navbar-toggler-icon"></span>
    </button>

    <!-- Collapsible content -->
    <div class="collapse navbar-collapse" id="navbarNav">
      <ul class="navbar-nav ms-auto">
        <li class="nav-item">
          <a class="nav-link active" aria-current="page" href="/">Home</a>
        </li>
        <li class="nav-item">
          <a class="nav-link" href="/about">About</a>
        </li>
        <li class="nav-item dropdown">
          <a
            class="nav-link dropdown-toggle"
            href="#"
            id="navbarDropdown"
            role="button"
            data-bs-toggle="dropdown"
            aria-expanded="false"
          >
            Products
          </a>
          <ul class="dropdown-menu" aria-labelledby="navbarDropdown">
            <li><a class="dropdown-item" href="/products/1">Product 1</a></li>
            <li><a class="dropdown-item" href="/products/2">Product 2</a></li>
            <li><hr class="dropdown-divider" /></li>
            <li><a class="dropdown-item" href="/products">All Products</a></li>
          </ul>
        </li>
      </ul>
    </div>
  </div>
</nav>
```

**Navbar Guidelines**:

- ✅ Use `navbar-expand-{breakpoint}` for responsive collapse behavior
- ✅ Include `aria-label` on toggle button for accessibility
- ✅ Use `ms-auto` or `me-auto` to align nav items right/left
- ✅ Mark current page with `active` class and `aria-current="page"`
- ❌ Don't nest multiple collapsible sections in one navbar
- ❌ Don't forget mobile testing for collapsed state

### Button Components

**Button Styles and Sizes**:

```html
<!-- Color variants -->
<button type="button" class="btn btn-primary">Primary</button>
<button type="button" class="btn btn-secondary">Secondary</button>
<button type="button" class="btn btn-success">Success</button>
<button type="button" class="btn btn-danger">Danger</button>
<button type="button" class="btn btn-warning">Warning</button>
<button type="button" class="btn btn-info">Info</button>
<button type="button" class="btn btn-light">Light</button>
<button type="button" class="btn btn-dark">Dark</button>
<button type="button" class="btn btn-link">Link</button>

<!-- Outline variants -->
<button type="button" class="btn btn-outline-primary">Outline Primary</button>

<!-- Sizes -->
<button type="button" class="btn btn-primary btn-lg">Large</button>
<button type="button" class="btn btn-primary">Default</button>
<button type="button" class="btn btn-primary btn-sm">Small</button>

<!-- Block buttons (full width) -->
<div class="d-grid gap-2">
  <button class="btn btn-primary" type="button">Block Button</button>
  <button class="btn btn-secondary" type="button">Block Button</button>
</div>

<!-- Button groups -->
<div class="btn-group" role="group" aria-label="Basic example">
  <button type="button" class="btn btn-primary">Left</button>
  <button type="button" class="btn btn-primary">Middle</button>
  <button type="button" class="btn btn-primary">Right</button>
</div>

<!-- Loading state -->
<button class="btn btn-primary" type="button" disabled>
  <span
    class="spinner-border spinner-border-sm"
    role="status"
    aria-hidden="true"
  ></span>
  Loading...
</button>
```

**Button Best Practices**:

- ✅ Use semantic color classes (`btn-primary`, `btn-danger`, etc.)
- ✅ Use `type="button"` for non-submit buttons
- ✅ Use `type="submit"` for form submission buttons
- ✅ Add `disabled` attribute for disabled state
- ✅ Include loading indicators for async operations
- ❌ Don't use `<div>` or `<span>` as buttons (use `<button>` or `<a>`)
- ❌ Don't forget focus states for keyboard navigation

### Forms

**Form Layout**:

```html
<form method="post">
  <!-- Basic input -->
  <div class="mb-3">
    <label for="email" class="form-label">Email address</label>
    <input
      type="email"
      class="form-control"
      id="email"
      name="email"
      placeholder="name@example.com"
      required
    />
    <div class="form-text">We'll never share your email.</div>
  </div>

  <!-- Select dropdown -->
  <div class="mb-3">
    <label for="country" class="form-label">Country</label>
    <select class="form-select" id="country" name="country" required>
      <option value="">Choose...</option>
      <option value="us">United States</option>
      <option value="ca">Canada</option>
      <option value="uk">United Kingdom</option>
    </select>
  </div>

  <!-- Checkbox -->
  <div class="mb-3 form-check">
    <input
      type="checkbox"
      class="form-check-input"
      id="terms"
      name="terms"
      required
    />
    <label class="form-check-label" for="terms">
      I agree to the terms and conditions
    </label>
  </div>

  <!-- Radio buttons -->
  <div class="mb-3">
    <label class="form-label">Subscription Plan</label>
    <div class="form-check">
      <input
        class="form-check-input"
        type="radio"
        name="plan"
        id="planFree"
        value="free"
        checked
      />
      <label class="form-check-label" for="planFree">Free</label>
    </div>
    <div class="form-check">
      <input
        class="form-check-input"
        type="radio"
        name="plan"
        id="planPro"
        value="pro"
      />
      <label class="form-check-label" for="planPro">Pro</label>
    </div>
  </div>

  <!-- Input group (with icon/button) -->
  <div class="mb-3">
    <label for="username" class="form-label">Username</label>
    <div class="input-group">
      <span class="input-group-text">@</span>
      <input
        type="text"
        class="form-control"
        id="username"
        name="username"
        placeholder="Username"
        required
      />
    </div>
  </div>

  <!-- Validation states -->
  <div class="mb-3">
    <label for="validInput" class="form-label">Valid Input</label>
    <input
      type="text"
      class="form-control is-valid"
      id="validInput"
      value="Valid"
    />
    <div class="valid-feedback">Looks good!</div>
  </div>

  <div class="mb-3">
    <label for="invalidInput" class="form-label">Invalid Input</label>
    <input type="text" class="form-control is-invalid" id="invalidInput" />
    <div class="invalid-feedback">Please provide a valid value.</div>
  </div>

  <!-- Submit button -->
  <button type="submit" class="btn btn-primary">Submit</button>
</form>
```

**Form Best Practices**:

- ✅ Always use `<label>` with `for` attribute matching input `id`
- ✅ Use `form-control` for text inputs, textareas, selects
- ✅ Use `form-check` for checkboxes and radio buttons
- ✅ Use `form-select` for select dropdowns
- ✅ Wrap each form group in `mb-3` (or appropriate spacing)
- ✅ Use `is-valid` and `is-invalid` classes with feedback divs
- ✅ Use `required` attribute for client-side validation
- ❌ Don't forget labels (accessibility requirement)
- ❌ Don't use placeholders as labels

### Cards

**Card Component**:

```html
<!-- Basic card -->
<div class="card">
  <img src="/image.jpg" class="card-img-top" alt="..." />
  <div class="card-body">
    <h5 class="card-title">Card Title</h5>
    <p class="card-text">Some quick example text to build on the card title.</p>
    <a href="#" class="btn btn-primary">Go somewhere</a>
  </div>
</div>

<!-- Card with header and footer -->
<div class="card">
  <div class="card-header">Featured</div>
  <div class="card-body">
    <h5 class="card-title">Special title</h5>
    <p class="card-text">Content goes here.</p>
    <a href="#" class="btn btn-primary">Button</a>
  </div>
  <div class="card-footer text-muted">2 days ago</div>
</div>

<!-- Card grid -->
<div class="row row-cols-1 row-cols-md-2 row-cols-lg-3 g-4">
  <div class="col">
    <div class="card h-100">
      <img src="/image1.jpg" class="card-img-top" alt="..." />
      <div class="card-body">
        <h5 class="card-title">Card 1</h5>
        <p class="card-text">This is card 1.</p>
      </div>
    </div>
  </div>
  <div class="col">
    <div class="card h-100">
      <img src="/image2.jpg" class="card-img-top" alt="..." />
      <div class="card-body">
        <h5 class="card-title">Card 2</h5>
        <p class="card-text">This is card 2.</p>
      </div>
    </div>
  </div>
  <!-- More cards... -->
</div>
```

**Card Guidelines**:

- ✅ Use `h-100` for equal-height cards in grid layouts
- ✅ Use `card-img-top`, `card-img-bottom`, or `card-img-overlay` for images
- ✅ Maintain consistent card structure across the application
- ✅ Use `text-center` in `card-body` for centered content
- ❌ Don't nest cards unnecessarily
- ❌ Don't forget alt text on card images

### Modals

**Modal Component**:

```html
<!-- Modal trigger button -->
<button
  type="button"
  class="btn btn-primary"
  data-bs-toggle="modal"
  data-bs-target="#exampleModal"
>
  Launch Modal
</button>

<!-- Modal -->
<div
  class="modal fade"
  id="exampleModal"
  tabindex="-1"
  aria-labelledby="exampleModalLabel"
  aria-hidden="true"
>
  <div class="modal-dialog">
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title" id="exampleModalLabel">Modal Title</h5>
        <button
          type="button"
          class="btn-close"
          data-bs-dismiss="modal"
          aria-label="Close"
        ></button>
      </div>
      <div class="modal-body">
        <p>Modal content goes here.</p>
        <form>
          <!-- Form fields -->
        </form>
      </div>
      <div class="modal-footer">
        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">
          Close
        </button>
        <button type="button" class="btn btn-primary">Save changes</button>
      </div>
    </div>
  </div>
</div>

<!-- Sizes -->
<div class="modal-dialog modal-sm">
  <!-- Small modal -->
  <div class="modal-dialog modal-lg">
    <!-- Large modal -->
    <div class="modal-dialog modal-xl">
      <!-- Extra large modal -->

      <!-- Vertically centered -->
      <div class="modal-dialog modal-dialog-centered">
        <!-- Scrollable -->
        <div class="modal-dialog modal-dialog-scrollable"></div>
      </div>
    </div>
  </div>
</div>
```

**Modal Best Practices**:

- ✅ Use unique `id` for each modal
- ✅ Include `aria-labelledby` pointing to modal title
- ✅ Use `tabindex="-1"` on modal container
- ✅ Include close button with `data-bs-dismiss="modal"`
- ✅ Use `modal-dialog-centered` for better UX
- ❌ Don't nest modals within modals
- ❌ Don't forget keyboard accessibility (ESC to close)

### Alerts

**Alert Component**:

```html
<!-- Basic alerts -->
<div class="alert alert-primary" role="alert">
  A simple primary alert—check it out!
</div>

<div class="alert alert-success" role="alert">
  <strong>Success!</strong> Your changes have been saved.
</div>

<div class="alert alert-danger" role="alert">
  <strong>Error!</strong> Something went wrong.
</div>

<!-- Dismissible alert -->
<div class="alert alert-warning alert-dismissible fade show" role="alert">
  <strong>Warning!</strong> This is a dismissible alert.
  <button
    type="button"
    class="btn-close"
    data-bs-dismiss="alert"
    aria-label="Close"
  ></button>
</div>

<!-- Alert with icon and link -->
<div class="alert alert-info d-flex align-items-center" role="alert">
  <svg class="bi flex-shrink-0 me-2" width="24" height="24">
    <use xlink:href="#info-fill" />
  </svg>
  <div>
    An info alert with <a href="#" class="alert-link">an example link</a>.
  </div>
</div>
```

**Alert Best Practices**:

- ✅ Always include `role="alert"` for accessibility
- ✅ Use semantic color classes matching the message type
- ✅ Add `alert-dismissible fade show` for dismissible alerts
- ✅ Use `<strong>` for emphasis in alert content
- ✅ Use `alert-link` class for links within alerts
- ❌ Don't overuse alerts (causes alert fatigue)
- ❌ Don't use alerts for static informational content

## JavaScript Component Management

### Script Loading Order

**Required Order**:

```html
<!-- 1. Bootstrap Bundle (includes Popper.js) -->
<script src="~/lib/bootstrap/dist/js/bootstrap.bundle.min.js"></script>

<!-- 2. Application JavaScript -->
<script src="~/js/site.js"></script>

<!-- 3. Page-specific scripts -->
@section Scripts {
<script src="~/js/pages/calculator.js"></script>
}
```

**Key Rules**:

- ✅ Load Bootstrap JS before custom scripts that use Bootstrap components
- ✅ Use `bootstrap.bundle.min.js` (includes Popper.js)
- ✅ Place scripts at end of `<body>` for better performance
- ❌ Don't load both bundle and separate Bootstrap + Popper files

### Component Initialization

**Data Attributes (Preferred Method)**:

```html
<!-- Most Bootstrap components work automatically with data attributes -->

<!-- Dropdown -->
<div class="dropdown">
  <button
    class="btn btn-secondary dropdown-toggle"
    type="button"
    data-bs-toggle="dropdown"
    aria-expanded="false"
  >
    Dropdown
  </button>
  <ul class="dropdown-menu">
    <li><a class="dropdown-item" href="#">Action</a></li>
  </ul>
</div>

<!-- Tooltip -->
<button
  type="button"
  class="btn btn-secondary"
  data-bs-toggle="tooltip"
  data-bs-placement="top"
  title="Tooltip on top"
>
  Hover me
</button>

<!-- Modal -->
<button
  type="button"
  class="btn btn-primary"
  data-bs-toggle="modal"
  data-bs-target="#myModal"
>
  Launch Modal
</button>

<!-- Collapse -->
<button
  class="btn btn-primary"
  type="button"
  data-bs-toggle="collapse"
  data-bs-target="#collapseExample"
  aria-expanded="false"
  aria-controls="collapseExample"
>
  Toggle Collapse
</button>
```

**JavaScript API (When More Control Needed)**:

```javascript
// Initialize tooltips (required for all tooltips)
document.addEventListener("DOMContentLoaded", function () {
  var tooltipTriggerList = [].slice.call(
    document.querySelectorAll('[data-bs-toggle="tooltip"]'),
  );
  var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
    return new bootstrap.Tooltip(tooltipTriggerEl);
  });
});

// Manually control a modal
const myModal = new bootstrap.Modal(document.getElementById("myModal"), {
  keyboard: false, // Disable ESC key
  backdrop: "static", // Prevent clicking backdrop to close
});

myModal.show(); // Show modal
myModal.hide(); // Hide modal

// Listen to modal events
document
  .getElementById("myModal")
  .addEventListener("shown.bs.modal", function (event) {
    console.log("Modal is now visible");
  });

// Programmatic collapse
const myCollapse = new bootstrap.Collapse(
  document.getElementById("collapseExample"),
  {
    toggle: false,
  },
);
myCollapse.show();

// Toast notification
const toastElement = document.getElementById("myToast");
const toast = new bootstrap.Toast(toastElement, {
  autohide: true,
  delay: 5000,
});
toast.show();

// Dismiss all alerts
document.querySelectorAll(".alert .btn-close").forEach(function (button) {
  button.addEventListener("click", function () {
    const alert = bootstrap.Alert.getOrCreateInstance(this.closest(".alert"));
    alert.close();
  });
});
```

**Component Options Configuration**:

```javascript
// Configure component via data attributes
<div
  class="modal"
  id="myModal"
  data-bs-backdrop="static"
  data-bs-keyboard="false"
></div>;

// Or via JavaScript options
const modal = new bootstrap.Modal(document.getElementById("myModal"), {
  backdrop: "static",
  keyboard: false,
});
```

### Event Handling

**Bootstrap Events**:

```javascript
// Modal events
const modalEl = document.getElementById("myModal");

// Before modal shows
modalEl.addEventListener("show.bs.modal", function (event) {
  console.log("About to show modal");
  // Can call event.preventDefault() to cancel
});

// After modal is visible
modalEl.addEventListener("shown.bs.modal", function (event) {
  console.log("Modal is now visible");
  document.getElementById("modalInput").focus();
});

// Before modal hides
modalEl.addEventListener("hide.bs.modal", function (event) {
  console.log("About to hide modal");
});

// After modal is hidden
modalEl.addEventListener("hidden.bs.modal", function (event) {
  console.log("Modal is hidden");
  // Clean up or reset form
});

// Dropdown events
document
  .getElementById("myDropdown")
  .addEventListener("show.bs.dropdown", function () {
    console.log("Dropdown opening");
  });

// Collapse events
document
  .getElementById("myCollapse")
  .addEventListener("shown.bs.collapse", function () {
    console.log("Collapse expanded");
  });

// Tab events
document
  .querySelectorAll('button[data-bs-toggle="tab"]')
  .forEach(function (tab) {
    tab.addEventListener("shown.bs.tab", function (event) {
      console.log("Active tab:", event.target); // newly activated tab
      console.log("Previous tab:", event.relatedTarget); // previous active tab
    });
  });
```

**Event Best Practices**:

- ✅ Use `show.bs.*` and `hide.bs.*` for before events
- ✅ Use `shown.bs.*` and `hidden.bs.*` for after events
- ✅ Clean up resources in `hidden.bs.*` events
- ✅ Use `event.preventDefault()` to cancel actions
- ❌ Don't forget to remove event listeners when dynamically creating/destroying components

### Dynamic Content

**Adding Components Dynamically**:

```javascript
// Function to add a new alert
function showAlert(message, type = "info") {
  const alertHTML = `
        <div class="alert alert-${type} alert-dismissible fade show" role="alert">
            ${message}
            <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
        </div>
    `;

  const container = document.getElementById("alert-container");
  container.insertAdjacentHTML("beforeend", alertHTML);

  // Auto-dismiss after 5 seconds
  const alerts = container.querySelectorAll(".alert");
  const newAlert = alerts[alerts.length - 1];
  setTimeout(() => {
    const bsAlert = bootstrap.Alert.getOrCreateInstance(newAlert);
    bsAlert.close();
  }, 5000);
}

// Usage
showAlert("Operation successful!", "success");
showAlert("An error occurred.", "danger");

// Dynamically create a modal
function createConfirmModal(title, message, onConfirm) {
  const modalId = "confirmModal-" + Date.now();
  const modalHTML = `
        <div class="modal fade" id="${modalId}" tabindex="-1">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title">${title}</h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
                    </div>
                    <div class="modal-body">${message}</div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
                        <button type="button" class="btn btn-primary" id="${modalId}-confirm">Confirm</button>
                    </div>
                </div>
            </div>
        </div>
    `;

  document.body.insertAdjacentHTML("beforeend", modalHTML);

  const modalEl = document.getElementById(modalId);
  const modal = new bootstrap.Modal(modalEl);

  // Handle confirm button
  document
    .getElementById(`${modalId}-confirm`)
    .addEventListener("click", function () {
      onConfirm();
      modal.hide();
    });

  // Clean up after modal is hidden
  modalEl.addEventListener("hidden.bs.modal", function () {
    modalEl.remove();
  });

  modal.show();
}

// Usage
createConfirmModal(
  "Delete Item",
  "Are you sure you want to delete this item?",
  function () {
    console.log("Item deleted");
  },
);
```

## Customization and Theming

### CSS Variables (Recommended for Bootstrap 5.2+)

**Theme Customization via CSS Variables**:

```css
/* site.css - Custom theme */
:root {
  /* Override Bootstrap CSS variables */
  --bs-primary: #0066cc;
  --bs-secondary: #6c757d;
  --bs-success: #28a745;
  --bs-danger: #dc3545;
  --bs-warning: #ffc107;
  --bs-info: #17a2b8;

  /* Typography */
  --bs-body-font-family: "Segoe UI", Tahoma, Geneva, Verdana, sans-serif;
  --bs-body-font-size: 1rem;
  --bs-body-line-height: 1.5;

  /* Spacing */
  --bs-gutter-x: 1.5rem;
  --bs-gutter-y: 1rem;

  /* Border radius */
  --bs-border-radius: 0.375rem;
  --bs-border-radius-lg: 0.5rem;
  --bs-border-radius-sm: 0.25rem;

  /* Shadows */
  --bs-box-shadow: 0 0.5rem 1rem rgba(0, 0, 0, 0.15);
  --bs-box-shadow-sm: 0 0.125rem 0.25rem rgba(0, 0, 0, 0.075);
  --bs-box-shadow-lg: 0 1rem 3rem rgba(0, 0, 0, 0.175);
}

/* Dark mode theming */
[data-bs-theme="dark"] {
  --bs-body-bg: #212529;
  --bs-body-color: #dee2e6;
  --bs-primary: #0d6efd;
}
```

**Apply Dark Mode**:

```html
<!-- Set theme on html element -->
<html lang="en" data-bs-theme="dark">
  <!-- Or toggle via JavaScript -->
  <script>
    function setTheme(theme) {
      document.documentElement.setAttribute("data-bs-theme", theme);
      localStorage.setItem("theme", theme);
    }

    // Load saved theme
    const savedTheme = localStorage.getItem("theme") || "light";
    setTheme(savedTheme);

    // Theme toggle button
    document
      .getElementById("theme-toggle")
      .addEventListener("click", function () {
        const currentTheme =
          document.documentElement.getAttribute("data-bs-theme");
        const newTheme = currentTheme === "dark" ? "light" : "dark";
        setTheme(newTheme);
      });
  </script>
</html>
```

### Sass Customization (Advanced)

**Custom Bootstrap Build**:

```scss
// custom-bootstrap.scss

// 1. Include functions and variables first
@import "node_modules/bootstrap/scss/functions";
@import "node_modules/bootstrap/scss/variables";

// 2. Override default variables
$primary: #0066cc;
$secondary: #6c757d;
$success: #28a745;
$danger: #dc3545;

$font-family-sans-serif: "Segoe UI", Tahoma, Geneva, Verdana, sans-serif;
$font-size-base: 1rem;
$line-height-base: 1.5;

$border-radius: 0.375rem;
$border-radius-lg: 0.5rem;
$border-radius-sm: 0.25rem;

$spacer: 1rem;
$spacers: (
  0: 0,
  1: $spacer * 0.25,
  2: $spacer * 0.5,
  3: $spacer,
  4: $spacer * 1.5,
  5: $spacer * 3,
  6: $spacer * 5, // Add custom spacer
);

// 3. Include Bootstrap
@import "node_modules/bootstrap/scss/bootstrap";

// 4. Add custom styles
.custom-component {
  background-color: $primary;
  padding: $spacer;
  border-radius: $border-radius;
}
```

**Compile Sass**:

```bash
# Using sass CLI
npm install -g sass
sass custom-bootstrap.scss wwwroot/css/bootstrap.min.css --style compressed

# Or add to package.json scripts
{
  "scripts": {
    "build:css": "sass --no-source-map --style compressed custom-bootstrap.scss wwwroot/css/bootstrap.min.css"
  }
}
```

### Custom Component Styles

**Extending Bootstrap Components**:

```css
/* site.css - Custom component extensions */

/* Custom button variant */
.btn-custom {
  --bs-btn-color: #fff;
  --bs-btn-bg: #6f42c1;
  --bs-btn-border-color: #6f42c1;
  --bs-btn-hover-color: #fff;
  --bs-btn-hover-bg: #5a32a3;
  --bs-btn-hover-border-color: #5a32a3;
}

/* Custom card styling */
.card-custom {
  border: none;
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
  transition:
    transform 0.2s,
    box-shadow 0.2s;
}

.card-custom:hover {
  transform: translateY(-4px);
  box-shadow: 0 8px 12px rgba(0, 0, 0, 0.15);
}

/* Custom navbar */
.navbar-custom {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
}

.navbar-custom .nav-link {
  color: rgba(255, 255, 255, 0.9);
}

.navbar-custom .nav-link:hover {
  color: #fff;
}

/* Custom form styling */
.form-control:focus {
  border-color: #0066cc;
  box-shadow: 0 0 0 0.25rem rgba(0, 102, 204, 0.25);
}
```

**Customization Best Practices**:

- ✅ Use CSS variables for theme consistency
- ✅ Extend Bootstrap classes rather than overriding
- ✅ Maintain semantic naming conventions
- ✅ Keep custom styles in separate file (`site.css`)
- ✅ Document custom component classes
- ❌ Don't modify Bootstrap source files directly
- ❌ Don't use `!important` unless absolutely necessary

## Accessibility Guidelines

### ARIA Attributes

**Required ARIA Patterns**:

```html
<!-- Navigation landmarks -->
<nav class="navbar" aria-label="Main navigation">
  <button
    class="navbar-toggler"
    aria-controls="navbarNav"
    aria-expanded="false"
    aria-label="Toggle navigation"
  ></button>
</nav>

<!-- Modal accessibility -->
<div
  class="modal"
  id="myModal"
  tabindex="-1"
  aria-labelledby="modalTitle"
  aria-hidden="true"
>
  <div class="modal-dialog">
    <div class="modal-content">
      <div class="modal-header">
        <h5 id="modalTitle">Modal Title</h5>
      </div>
    </div>
  </div>
</div>

<!-- Dropdown menu -->
<button
  class="btn dropdown-toggle"
  id="dropdownMenuButton"
  data-bs-toggle="dropdown"
  aria-expanded="false"
>
  Dropdown
</button>
<ul class="dropdown-menu" aria-labelledby="dropdownMenuButton">
  <li><a class="dropdown-item" href="#">Action</a></li>
</ul>

<!-- Alert with icon -->
<div class="alert alert-warning" role="alert">
  <svg aria-hidden="true" focusable="false">...</svg>
  Warning message
</div>

<!-- Loading spinner -->
<div class="spinner-border" role="status">
  <span class="visually-hidden">Loading...</span>
</div>

<!-- Tabs -->
<ul class="nav nav-tabs" role="tablist">
  <li class="nav-item" role="presentation">
    <button
      class="nav-link active"
      id="home-tab"
      data-bs-toggle="tab"
      data-bs-target="#home"
      type="button"
      role="tab"
      aria-controls="home"
      aria-selected="true"
    >
      Home
    </button>
  </li>
</ul>
<div class="tab-content">
  <div
    class="tab-pane fade show active"
    id="home"
    role="tabpanel"
    aria-labelledby="home-tab"
  >
    Content
  </div>
</div>
```

### Keyboard Navigation

**Ensure Keyboard Accessibility**:

```html
<!-- All interactive elements must be keyboard accessible -->

<!-- ✅ Good: Native button is keyboard accessible -->
<button class="btn btn-primary" type="button">Click Me</button>

<!-- ❌ Bad: Div with click handler is not keyboard accessible -->
<div class="btn btn-primary" onclick="doSomething()">Click Me</div>

<!-- ✅ Good: If you must use non-button element, add proper attributes -->
<div
  class="btn btn-primary"
  role="button"
  tabindex="0"
  onclick="doSomething()"
  onkeypress="if(event.key==='Enter'||event.key===' ') doSomething()"
>
  Click Me
</div>

<!-- Skip to main content link -->
<a href="#main-content" class="visually-hidden-focusable"
  >Skip to main content</a
>
<main id="main-content">
  <!-- Main content -->
</main>
```

**Focus Management**:

```javascript
// Set focus when modal opens
document
  .getElementById("myModal")
  .addEventListener("shown.bs.modal", function () {
    document.getElementById("modalInput").focus();
  });

// Trap focus within modal (Bootstrap handles this automatically)

// Return focus when modal closes (Bootstrap handles this automatically)

// Custom focus trap for dropdowns
document.querySelectorAll(".dropdown").forEach(function (dropdown) {
  dropdown.addEventListener("shown.bs.dropdown", function () {
    const firstItem = dropdown.querySelector(".dropdown-item");
    if (firstItem) firstItem.focus();
  });
});
```

### Screen Reader Support

**Best Practices**:

```html
<!-- Visually hidden text for screen readers -->
<button class="btn btn-primary">
  <svg aria-hidden="true">...</svg>
  <span class="visually-hidden">Delete</span>
</button>

<!-- Or use sr-only-focusable for skip links -->
<a href="#main" class="visually-hidden-focusable">Skip to main content</a>

<!-- Describe icon buttons -->
<button class="btn btn-link" aria-label="Close modal">
  <svg aria-hidden="true">×</svg>
</button>

<!-- Live regions for dynamic content -->
<div aria-live="polite" aria-atomic="true" id="notifications">
  <!-- Alerts appear here -->
</div>

<!-- Form validation announcements -->
<div class="mb-3">
  <label for="email">Email</label>
  <input
    type="email"
    id="email"
    class="form-control is-invalid"
    aria-describedby="emailError"
  />
  <div id="emailError" class="invalid-feedback" role="alert">
    Please enter a valid email address.
  </div>
</div>
```

### Color Contrast

**WCAG Compliance**:

- ✅ Ensure text color contrast ratio ≥ 4.5:1 (normal text)
- ✅ Ensure text color contrast ratio ≥ 3:1 (large text, 18pt+)
- ✅ Test all custom color combinations
- ✅ Don't rely on color alone to convey information

```css
/* ✅ Good: Sufficient contrast */
.text-on-primary {
    background-color: #0066cc;
    color: #ffffff;  /* Contrast ratio: 4.67:1 */
}

/* ❌ Bad: Insufficient contrast */
.text-on-light {
    background-color: #f8f9fa;
    color: #adb5bd;  /* Contrast ratio: 2.5:1 - fails WCAG */
}

/* ✅ Good: Icon with visible label */
<button class="btn btn-danger">
    <svg aria-hidden="true">🗑️</svg>
    Delete
</button>

/* ❌ Bad: Icon only without accessible text */
<button class="btn btn-danger" title="Delete">
    🗑️
</button>
```

## Performance Optimization

### Bundle Size Optimization

**1. Use Only What You Need**:

```javascript
// ❌ Bad: Importing entire Bootstrap bundle
import "bootstrap";

// ✅ Good: Import only required components
import { Modal, Dropdown, Tooltip } from "bootstrap";
```

**2. Tree Shaking (Webpack/Vite)**:

```javascript
// webpack.config.js or vite.config.js
export default {
  optimization: {
    usedExports: true,
    sideEffects: false,
  },
};
```

**3. CSS Purging (Remove Unused Styles)**:

```javascript
// Use PurgeCSS or similar
// postcss.config.js
module.exports = {
  plugins: [
    require("@fullhuman/postcss-purgecss")({
      content: ["./Pages/**/*.cshtml", "./wwwroot/js/**/*.js"],
      defaultExtractor: (content) => content.match(/[\w-/:]+(?<!:)/g) || [],
    }),
  ],
};
```

### Lazy Loading

**Load JavaScript On-Demand**:

```javascript
// Load Bootstrap components only when needed
async function showModal(modalId) {
  // Lazy load Modal component
  const { Modal } = await import("bootstrap/js/dist/modal");
  const modal = new Modal(document.getElementById(modalId));
  modal.show();
}

// Intersection Observer for lazy component initialization
const observer = new IntersectionObserver((entries) => {
  entries.forEach((entry) => {
    if (entry.isIntersecting) {
      // Initialize tooltip when element is in viewport
      const tooltip = new bootstrap.Tooltip(entry.target);
      observer.unobserve(entry.target);
    }
  });
});

document.querySelectorAll('[data-bs-toggle="tooltip"]').forEach((el) => {
  observer.observe(el);
});
```

### Image Optimization

```html
<!-- Responsive images with Bootstrap -->
<img
  src="/image.jpg"
  class="img-fluid"
  alt="Description"
  loading="lazy"
  width="800"
  height="600"
/>

<!-- Srcset for different screen densities -->
<img
  src="/image.jpg"
  srcset="/image-1x.jpg 1x, /image-2x.jpg 2x"
  class="img-fluid"
  alt="Description"
/>

<!-- Picture element for art direction -->
<picture>
  <source media="(min-width: 768px)" srcset="/image-desktop.jpg" />
  <source media="(min-width: 576px)" srcset="/image-tablet.jpg" />
  <img src="/image-mobile.jpg" class="img-fluid" alt="Description" />
</picture>
```

### CSS Performance

```css
/* Reduce CSS specificity for better performance */

/* ❌ Bad: High specificity */
div.container div.row div.col .card .card-body p.text {
  color: #333;
}

/* ✅ Good: Low specificity */
.card-text {
  color: #333;
}

/* Use will-change for animations */
.modal.fade {
  will-change: transform, opacity;
}

/* Remove will-change after animation */
.modal.show {
  will-change: auto;
}
```

## Common Patterns and Examples

### Page Layout Template

**Full Page Layout**:

```html
<!DOCTYPE html>
<html lang="en" data-bs-theme="light">
  <head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>My App</title>
    <link rel="stylesheet" href="~/lib/bootstrap/dist/css/bootstrap.min.css" />
    <link rel="stylesheet" href="~/css/site.css" />
  </head>
  <body class="d-flex flex-column min-vh-100">
    <!-- Header -->
    <header>
      <nav class="navbar navbar-expand-lg navbar-dark bg-primary">
        <div class="container-fluid">
          <a class="navbar-brand" href="/">My App</a>
          <button
            class="navbar-toggler"
            type="button"
            data-bs-toggle="collapse"
            data-bs-target="#navbarNav"
          >
            <span class="navbar-toggler-icon"></span>
          </button>
          <div class="collapse navbar-collapse" id="navbarNav">
            <ul class="navbar-nav ms-auto">
              <li class="nav-item">
                <a class="nav-link" href="/">Home</a>
              </li>
              <li class="nav-item">
                <a class="nav-link" href="/about">About</a>
              </li>
            </ul>
          </div>
        </div>
      </nav>
    </header>

    <!-- Main content -->
    <main class="flex-grow-1">
      <div class="container my-4">
        <!-- Page content -->
        <h1>Welcome</h1>
        <p class="lead">This is a Bootstrap 5 template.</p>
      </div>
    </main>

    <!-- Footer -->
    <footer class="footer mt-auto py-3 bg-light">
      <div class="container text-center">
        <span class="text-muted">&copy; 2026 My App. All rights reserved.</span>
      </div>
    </footer>

    <script src="~/lib/bootstrap/dist/js/bootstrap.bundle.min.js"></script>
    <script src="~/js/site.js"></script>
  </body>
</html>
```

### Form with Validation

**Complete Form Pattern**:

```html
<div class="container my-5">
  <div class="row justify-content-center">
    <div class="col-md-8 col-lg-6">
      <div class="card">
        <div class="card-body">
          <h3 class="card-title mb-4">Contact Us</h3>
          <form id="contactForm" class="needs-validation" novalidate>
            <div class="mb-3">
              <label for="name" class="form-label">Name</label>
              <input type="text" class="form-control" id="name" required />
              <div class="invalid-feedback">Please enter your name.</div>
            </div>

            <div class="mb-3">
              <label for="email" class="form-label">Email</label>
              <input type="email" class="form-control" id="email" required />
              <div class="invalid-feedback">Please enter a valid email.</div>
            </div>

            <div class="mb-3">
              <label for="message" class="form-label">Message</label>
              <textarea
                class="form-control"
                id="message"
                rows="4"
                required
              ></textarea>
              <div class="invalid-feedback">Please enter your message.</div>
            </div>

            <div class="d-grid">
              <button type="submit" class="btn btn-primary">
                Send Message
              </button>
            </div>
          </form>
        </div>
      </div>
    </div>
  </div>
</div>

<script>
  // Form validation
  (function () {
    "use strict";

    const form = document.getElementById("contactForm");

    form.addEventListener(
      "submit",
      function (event) {
        if (!form.checkValidity()) {
          event.preventDefault();
          event.stopPropagation();
        } else {
          event.preventDefault();
          // Handle form submission
          submitForm();
        }

        form.classList.add("was-validated");
      },
      false,
    );

    async function submitForm() {
      const submitButton = form.querySelector('button[type="submit"]');
      submitButton.disabled = true;
      submitButton.innerHTML =
        '<span class="spinner-border spinner-border-sm me-2"></span>Sending...';

      try {
        // Simulate API call
        await new Promise((resolve) => setTimeout(resolve, 2000));
        showAlert("Message sent successfully!", "success");
        form.reset();
        form.classList.remove("was-validated");
      } catch (error) {
        showAlert("Failed to send message. Please try again.", "danger");
      } finally {
        submitButton.disabled = false;
        submitButton.innerHTML = "Send Message";
      }
    }

    function showAlert(message, type) {
      const alert = document.createElement("div");
      alert.className = `alert alert-${type} alert-dismissible fade show`;
      alert.role = "alert";
      alert.innerHTML = `
            ${message}
            <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
        `;
      form.insertAdjacentElement("beforebegin", alert);
    }
  })();
</script>
```

### Data Table with Actions

```html
<div class="container my-4">
  <div class="card">
    <div class="card-header d-flex justify-content-between align-items-center">
      <h5 class="mb-0">Users</h5>
      <button
        class="btn btn-primary btn-sm"
        data-bs-toggle="modal"
        data-bs-target="#addUserModal"
      >
        Add User
      </button>
    </div>
    <div class="card-body">
      <div class="table-responsive">
        <table class="table table-hover">
          <thead>
            <tr>
              <th>Name</th>
              <th>Email</th>
              <th>Role</th>
              <th>Status</th>
              <th>Actions</th>
            </tr>
          </thead>
          <tbody>
            <tr>
              <td>John Doe</td>
              <td>john@example.com</td>
              <td><span class="badge bg-primary">Admin</span></td>
              <td><span class="badge bg-success">Active</span></td>
              <td>
                <div class="btn-group btn-group-sm" role="group">
                  <button
                    type="button"
                    class="btn btn-outline-primary"
                    data-bs-toggle="tooltip"
                    title="Edit"
                  >
                    <svg width="16" height="16">✏️</svg>
                  </button>
                  <button
                    type="button"
                    class="btn btn-outline-danger"
                    data-bs-toggle="tooltip"
                    title="Delete"
                  >
                    <svg width="16" height="16">🗑️</svg>
                  </button>
                </div>
              </td>
            </tr>
            <!-- More rows -->
          </tbody>
        </table>
      </div>

      <!-- Pagination -->
      <nav aria-label="Page navigation">
        <ul class="pagination justify-content-center mb-0">
          <li class="page-item disabled">
            <a class="page-link" href="#" tabindex="-1">Previous</a>
          </li>
          <li class="page-item active"><a class="page-link" href="#">1</a></li>
          <li class="page-item"><a class="page-link" href="#">2</a></li>
          <li class="page-item"><a class="page-link" href="#">3</a></li>
          <li class="page-item">
            <a class="page-link" href="#">Next</a>
          </li>
        </ul>
      </nav>
    </div>
  </div>
</div>
```

## Troubleshooting

### Common Issues

**1. JavaScript Components Not Working**:

```javascript
// Problem: Components not initialized
// Solution: Ensure scripts loaded after DOM ready

// ❌ Bad: Running before DOM is ready
const modal = new bootstrap.Modal(document.getElementById("myModal"));

// ✅ Good: Wait for DOM
document.addEventListener("DOMContentLoaded", function () {
  const modal = new bootstrap.Modal(document.getElementById("myModal"));
});
```

**2. Tooltips/Popovers Not Showing**:

```javascript
// Problem: Tooltips require manual initialization
// Solution: Initialize all tooltips

// Initialize on page load
document.addEventListener("DOMContentLoaded", function () {
  var tooltipTriggerList = [].slice.call(
    document.querySelectorAll('[data-bs-toggle="tooltip"]'),
  );
  tooltipTriggerList.map(function (tooltipTriggerEl) {
    return new bootstrap.Tooltip(tooltipTriggerEl);
  });
});
```

**3. Modal Backdrop Remains After Close**:

```javascript
// Problem: Modal not properly disposed
// Solution: Clean up modals

document
  .getElementById("myModal")
  .addEventListener("hidden.bs.modal", function () {
    // Remove any remaining backdrops
    document.querySelectorAll(".modal-backdrop").forEach((el) => el.remove());
    document.body.classList.remove("modal-open");
    document.body.style.overflow = "";
  });
```

**4. Responsive Grid Not Working**:

```html
<!-- Problem: Missing viewport meta tag -->
<!-- Solution: Add to <head> -->
<meta name="viewport" content="width=device-width, initial-scale=1.0" />
```

**5. Custom Styles Not Applied**:

```css
/* Problem: Bootstrap specificity wins */
/* Solution: Increase specificity or use !important sparingly */

/* ❌ Bad: Too low specificity */
.card {
  background: red;
}

/* ✅ Good: Higher specificity */
.card.card-custom {
  background: red;
}

/* Or load custom CSS after Bootstrap */
```

## Migration Guide

### Upgrading from Bootstrap 4 to Bootstrap 5

**Major Changes**:

1. **jQuery Removed**: Bootstrap 5 uses vanilla JavaScript
2. **Dropped IE Support**: IE 10 and 11 no longer supported
3. **Form Elements**: New `.form-control`, `.form-select`, `.form-check` classes
4. **Utilities API**: New utility API for customization
5. **Grid System**: New `xxl` breakpoint added

**Breaking Changes Checklist**:

```html
<!-- Bootstrap 4 → Bootstrap 5 -->

<!-- Forms -->
<div class="form-group">
  →
  <div class="mb-3">
    <input class="form-control" /> → <input class="form-control" /> (same)
    <select class="form-control">
      →
      <select class="form-select">
        <input type="checkbox" class="form-check-input" />
        → (same)

        <!-- Buttons -->
        <button class="btn-block">
          →
          <button class="btn w-100">
            or
            <div class="d-grid">
              <!-- Utilities -->
              <div class="ml-3">
                →
                <div class="ms-3">
                  <div class="mr-3">
                    →
                    <div class="me-3">
                      <div class="pl-3">
                        →
                        <div class="ps-3">
                          <div class="pr-3">
                            →
                            <div class="pe-3">
                              <div class="font-weight-bold">
                                →
                                <div class="fw-bold">
                                  <div class="text-left">
                                    →
                                    <div class="text-start">
                                      <div class="text-right">
                                        →
                                        <div class="text-end">
                                          <!-- Forms -->
                                          <div
                                            class="custom-control custom-checkbox"
                                          >
                                            →
                                            <div class="form-check">
                                              <!-- Data attributes -->
                                              data-toggle="modal" →
                                              data-bs-toggle="modal"
                                              data-target="#myModal" →
                                              data-bs-target="#myModal"

                                              <!-- JavaScript -->
                                              $('#myModal').modal('show') → var
                                              modal = new
                                              bootstrap.Modal(document.getElementById('myModal'));
                                              modal.show();
                                            </div>
                                          </div>
                                        </div>
                                      </div>
                                    </div>
                                  </div>
                                </div>
                              </div>
                            </div>
                          </div>
                        </div>
                      </div>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </button>
        </button>
      </select>
    </select>
  </div>
</div>
```

## Summary

### Key Takeaways

1. **Use Bootstrap Utilities First**: Leverage utility classes before writing custom CSS
2. **Responsive by Default**: Always implement mobile-first responsive design
3. **Accessibility Matters**: Include proper ARIA attributes and semantic HTML
4. **Performance**: Optimize bundle size and lazy load when possible
5. **Consistency**: Follow Bootstrap conventions for maintainable code

### Quick Reference

**Most Used Classes**:

```
Layout:      .container, .row, .col-*
Spacing:     .m-*, .p-*, .mt-*, .mb-*, .mx-*, .my-*
Display:     .d-flex, .d-grid, .d-none, .d-block
Alignment:   .justify-content-*, .align-items-*, .text-center
Colors:      .bg-*, .text-*, .border-*
Components:  .btn, .card, .modal, .navbar, .alert, .badge
```

**Common Patterns**:

- Centered container: `.container` + `.row` + `.justify-content-center`
- Sticky footer: `<body class="d-flex flex-column min-vh-100">` + `<main class="flex-grow-1">`
- Card grid: `.row row-cols-*` + `.col` + `.card.h-100`
- Form layout: `.mb-3` + `.form-label` + `.form-control`

---

**Document Version**: 1.0.0
**Last Updated**: 2026-02-10
**Bootstrap Version**: 5.3.x
**Next Review**: 2026-05-10 (Quarterly)
**Feedback**: Submit improvements via PR or issue
