module Ai

open Browser.Types
open Locations
open System
open Audio

type xMaxType =
    | One
    | Two

let getNextRooms leftDoor rightDoor =
    function
    | RightDoor -> [ if not rightDoor then SecurityOffice else CenterBackStage ]
    | LeftDoor -> [ if not leftDoor then SecurityOffice else CenterBackStage ]
    | CenterBackStage -> [ RightBackStage; LeftBackStage ]
    | RightCorridor -> [ Kitchen; RightDoor; BossOffice ]
    | Kitchen -> [ RightCorridor; LeftCorridor ]
    | EntryRoom -> [ BossOffice; RightCorridor ]
    | RightHall -> [ RightCorridor; EntryRoom ]
    | LeftHall -> [ RightHall; LeftCorridor ]
    | LeftCorridor -> [ Kitchen; LeftDoor ]
    | SecurityOffice -> [ SecurityOffice ]
    | RightBackStage -> [ RightStage ]
    | LeftBackStage -> [ LeftStage ]
    | RightStage -> [ RightHall ]
    | BossOffice -> [ RightDoor ]
    | Vent -> [ SecurityOffice ]
    | LeftStage -> [ LeftHall ]

let getLocationDuration =
    function
    | LeftCorridor -> 40
    | RightCorridor -> 40
    | EntryRoom -> 20
    | Kitchen -> 70
    | RightHall -> 30
    | LeftHall -> 30
    | CenterBackStage -> 30
    | BossOffice -> 20
    | RightBackStage -> 50
    | LeftBackStage -> 50
    | RightStage -> 80
    | LeftStage -> 80
    | RightDoor -> 20
    | LeftDoor -> 20
    | Vent -> 150
    | SecurityOffice -> 1000

type xMax =
    { t: xMaxType
      location: location
      dur: int }

let makeStep (xMaxLoc: xMax) (leftClosed: bool) (rightClosed: bool) : xMax =
    let rand = Random()

    let pickRandom lst =
        List.item (rand.Next() % (List.length lst)) lst

    match xMaxLoc.dur with
    | 0 ->
        let newLoc = getNextRooms leftClosed rightClosed xMaxLoc.location |> pickRandom
        let newDur = getLocationDuration newLoc
        
        if newLoc = CenterBackStage && (xMaxLoc.location = RightDoor || xMaxLoc.location = LeftDoor) then
            playAudio DoorKnock

        { xMaxLoc with
            location = newLoc
            dur = newDur }
    | n -> { xMaxLoc with dur = n - 1 }

let printXMax xMax =
    printfn $"%s{string xMax}"

let initialXMaxes =
    [ { t = One
        location = RightStage
        dur = 30 }
      { t = Two
        location = LeftStage
        dur = 20 } ]
