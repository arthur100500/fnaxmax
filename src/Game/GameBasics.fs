module GameBasics

open Ai
open CameraState

type doorStage = int

type doorStatus =
    | Open
    | Closed
    | Opening of doorStage
    | Closing of doorStage

type userControls =
    { mutable cameraSwitch: bool
      mutable cameraLeft: bool
      mutable cameraRight: bool
      mutable lightLeft: bool
      mutable lightRight: bool }

type gameState =
    { pov: pointOfView
      leftDoor: doorStatus
      rightDoor: doorStatus
      time: float
      tickNum: int
      enemies: xMax list
      leftLight: bool
      rightLight: bool }

let initGameState =
    { pov = CamLeftCorridor, InOffice
      leftDoor = Open
      rightDoor = Open
      time = 0.
      tickNum = 0
      enemies = initialXMaxes
      leftLight = false
      rightLight = false }

let globalControls =
    { cameraSwitch = false
      cameraLeft = false
      cameraRight = false
      lightLeft = false
      lightRight = false }
