# Agent Guidelines for vitaaid.com

## Build/Lint/Test Commands
- **Build**: `npm run build` (ClientApp directory)
- **Dev server**: `npm start` (ClientApp directory)
- **Test all**: `npm test` (ClientApp directory)
- **Single test**: `npm test -- --testNamePattern="pattern"` or `npm test -- --testPathPattern=filename`
- **Lint**: ESLint configured in package.json (extends react-app, react-app/jest)

## Search Tools
- **ripgrep (rg)**: Fast text search tool with regex support
- **Regex Limitations**: ripgrep uses Rust regex engine which does NOT support:
  - Non-capturing groups `(?:...)`
  - Unclosed parentheses will cause "unclosed group" errors
  - Use simple patterns like `pattern1.*pattern2` instead of complex groupings

## Code Style Guidelines

### Formatting
- Prettier config: printWidth 100, single quotes, semicolons, 2-space tabs, trailing commas "all"
- End of line: auto

### TypeScript
- Strict mode enabled
- JSX transform: react-jsx
- Base URL: src (allows absolute imports like `import from 'model/Product'`)

### Naming Conventions
- **Components**: PascalCase (e.g., `ProductCard`, `ShoppingCartPage`)
- **Interfaces**: PascalCase (e.g., `ProductData`, `UserInfo`)
- **Functions/Variables**: camelCase (e.g., `getProducts`, `handleSubmit`)
- **Classes**: PascalCase (C# backend)

### Imports
- React imports first
- Third-party libraries second
- Local imports last (use absolute paths from src/)

### React Patterns
- Functional components with hooks
- @emotion/react for styling (css prop)
- React Hook Form for form handling
- Redux Toolkit for state management

### Error Handling
- Use try/catch blocks
- Return empty arrays/objects on API errors rather than throwing
- Use non-null assertion (!) for required environment variables

### Backend (C#)
- PascalCase for classes and properties
- NHibernate with virtual properties
- ASP.NET Core dependency injection
- Standard MVC patterns