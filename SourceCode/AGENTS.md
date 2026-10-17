# C# conventions

- Use explicit types instead of `var` wherever the type can be named.
- Use file-scoped namespaces for namespace declarations.
- Declare only one top-level class per file. Name the file after the class.
- Keep these conventions enforced during builds through `.editorconfig` and `Directory.Build.props`.
