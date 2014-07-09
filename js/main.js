
if (document.getElementsByTagName("body")[0].className.indexOf("xp") > -1) {
  var download = document.getElementById("download"),
      downloadAlt = document.getElementById("download-alt"),
      screenshot = document.getElementById("screenshot");

  download.href = "download-xp.html";
  downloadAlt.href = "download-vista.html";
  screenshot.src = "img/screenshot_xp.png";
}
