app = require "app"
BrowserWindow = require "browser-window"

# Report crashes to our server.
# require("crash-reporter").start()

# Keep a global reference of the window object, if you don't, the window will
# be closed automatically when the javascript object is GCed.
mainWindow = null

# Quit when all windows are closed.
app.on "window-all-closed", ->
  app.quit()


# This method will be called when atom-shell has done everything
# initialization and ready for creating browser windows.
app.on "ready", ->
  # Create the browser window.
  mainWindow = new BrowserWindow(
    "width": 800
    "height": 600
    "min-width": 400
    "min-height": 300
    "frame": false
    "direct-write": true
  )
  # and load the index.html of the app.
  mainWindow.loadUrl "file://" + __dirname + "/index.html"
  # mainWindow.openDevTools()

  mainWindow.on "close", ->
    app.quit()

  ipc = require "ipc"
  ipc.on "minimize", ->
    mainWindow.minimize()
  return
