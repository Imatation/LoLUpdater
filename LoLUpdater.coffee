$ = document.querySelector
gui = require("nw.gui")
LoLUpdater =
  init: ->
    $("go").addEventListener "click", ->
      $(".working").toggle()
      return
    $(".exit").addEventListener "click", ->
      gui.App.quit()
      return
    $(".bugs").addEventListener "click", ->
      gui.Shell.openExternal "https://github.com/Loggan08/LoLUpdater/issues"
      return
    return

LoLUpdater.init()
