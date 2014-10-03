# TODO: create "watch" task
gulp = require("gulp")
$ = require("gulp-load-plugins")()
del = require("del")

gulp.task "default", ["dist"]

gulp.task "clean", ->
  del([
    "atom-shell/resources/app/**"
    ])

gulp.task "dist", ->
  autoPrefixerOptions =
    browsers: ["Chrome 37"]
    cascade: false
  coffeeFilter = $.filter("**/*.coffee")
  cssFilter = $.filter("**/*.css")
  jsonFilter = $.filter("**/*.json")
  gulp.src([
      "src/**/*.*"
      "!src/**/.*"
      "!src/**/test"
      "!src/**/Thumbs.db"
      "!src/components/**/bower.json"
      "!src/components/**/demo.html"
      "!src/components/**/index.html"
      "!src/components/**/metadata.html"
      "!src/components/**/README.md"
    ])
    .pipe(coffeeFilter)
    .pipe($.coffee(bare: true))
    .pipe($.uglify())
    .pipe(coffeeFilter.restore())
    .pipe(jsonFilter)
    .pipe($.jsonminify())
    .pipe(jsonFilter.restore())
    .pipe(gulp.dest("atom-shell/resources/app"))

gulp.task "downloadAtomShell", ->
  downloadOptions =
    version: "0.17.1"
    outputDir: "atom-shell"
  $.downloadAtomShell(downloadOptions)
