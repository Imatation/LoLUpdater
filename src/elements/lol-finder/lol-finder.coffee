Polymer "lol-finder",
  ready: ->
    self = this
    @value = localStorage["LoLPath"] if localStorage["LoLPath"]?
    @$.button.addEventListener "click", ->
      remote = require("remote")
      dialog = remote.require("dialog")
      browserWindow = remote.getCurrentWindow()
      dialog.showOpenDialog browserWindow,
        title: "Find LoL folder"
        defaultPath: @value or "/"
        properties: ["openDirectory"]
      , (filenames) ->
        self.value = filenames[0] if filenames[0]?
