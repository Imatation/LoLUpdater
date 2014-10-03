addEventListener "DOMContentLoaded", ->
  remote = require "remote"

  exitApp = ->
    app = remote.require "app"
    app.quit()

  minimizeApp = ->
    ipc = require "ipc"
    ipc.send "minimize"

  document.getElementById("patch").addEventListener "click", ->
    document.querySelector(".patch-dialog").toggle()
    localStorage["LoLPath"] = document.querySelector("lol-finder").value

  for exit in document.querySelectorAll(".exit")
    exit.addEventListener "click", exitApp

  document.querySelector(".minimize").addEventListener "click", minimizeApp

  document.querySelector(".bug").addEventListener "click", ->
    shell = require "shell"
    shell.openExternal "https://github.com/Loggan08/LoLUpdater/issues"
