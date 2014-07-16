'use strict';

var gulp = require('gulp');
var $ = require('gulp-load-plugins')();

var paths = {
  scripts: ['js/*.coffee'],
  styles: 'sass/*.sass'
};

/*gulp.task('scripts', function() {
  gulp.src(paths.scripts)
    .pipe($.coffee())
    .pipe($.uglify())
    .pipe($.concat('main.js'))
    .pipe(gulp.dest('js'));
});*/

gulp.task('styles', function() {
  gulp.src(paths.styles)
    .pipe($.rubySass({
      "style": "compressed"
    }))
    .pipe($.autoprefixer())
    .pipe(gulp.dest('styles'));
});

gulp.task('default', ['styles']);
