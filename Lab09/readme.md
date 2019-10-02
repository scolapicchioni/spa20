# NOTE: THIS REPO IS OUTDATED. THE NEW VERSION USES VUE CLI 3.11 AND .NET CORE 3.0. REFER TO THE NEW REPO FOR THE NEW STEPS. https://github.com/scolapicchioni/spa30

# PWA

We're going to make use of the PWA features.

- manifest.json
  - "background_color": "#fff",
  - "theme_color": "#FFC107"
- index.html
  - <meta name="theme-color" content="#FFC107">
  - <meta name="msapplication-TileColor" content="#FFC107">
- npm run build
- npm install -g serve
- serve --port 5001 dist/ 
