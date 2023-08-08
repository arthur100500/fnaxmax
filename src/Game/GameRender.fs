module GameRender

open GameBasics
open Render
open RenderBasics
open Ai
open Textures
open Locations
open CameraState

let inline renderRoom (gs: gameState) rd =
    let drawRoom location =
        let locationString =
            match location with
            | CamLeftCorridor when gs.leftDoor = Open -> LLeftCorridorOpen
            | CamRightCorridor when gs.rightDoor = Open -> LRightCorridorOpen
            | CamLeftCorridor -> LLeftCorridorClosed
            | CamRightCorridor -> LRightCorridorClosed
            | CamBossOffice -> LBossOffice
            | CamEntryRoom -> LEntryRoom
            | CamLeftHall -> LLeftHall
            | CamRightHall -> LRightHall
            | CamLeftBackStage -> LLeftBackStage
            | CamRightBackStage -> LRightBackStage
            | CamKitchen -> LKitchen
        drawStillImage locationString rd (programWithStatic rd)
        
        let inline drawMax (m: xMax) =
            let loc = m.location
            let maxImage =
                match loc, location with
                | Kitchen, CamKitchen -> Some XKitchen
                | BossOffice, CamBossOffice -> Some XBossOffice
                | LeftHall, CamLeftHall -> Some XLeftHall
                | RightCorridor, CamRightCorridor -> Some XRightCorridor
                | LeftBackStage, CamLeftBackStage -> Some XLeftBackStage
                | LeftStage, CamLeftHall -> Some XLeftStage
                | RightStage, CamRightHall -> Some XRightStage
                | LeftCorridor, CamLeftCorridor -> Some XLeftCorridor
                | RightHall, CamRightHall -> Some XRightHall
                | EntryRoom, CamEntryRoom -> Some XEntryRoom
                | RightBackStage, CamRightBackStage -> Some XRightBackStage
                | _ -> None
            match maxImage with
            | Some img ->
                drawStillImage img rd (programWithStatic rd)
            | None -> ()
        
        List.iter drawMax gs.enemies

    let inline drawDoor img1 img2 img3 =
        function
        | Open -> ()
        | Closing 0 -> ()
        | Opening 0 -> ()
        | Closing 3
        | Opening 3 -> drawStillImage img3 rd (usualProgram rd)
        | Closing 2
        | Opening 2 -> drawStillImage img2 rd (usualProgram rd)
        | Closing 1
        | Opening 1 -> drawStillImage img1 rd (usualProgram rd)
        | Closed -> drawStillImage img3 rd (usualProgram rd)   
        | _ -> failwith "unreachable"
    
    let inline drawLight () =
        if gs.leftLight then
            drawStillImage OLeftLight rd (usualProgram rd)
        if gs.rightLight then
            drawStillImage ORightLight rd (usualProgram rd)
            
    let inline drawDoorMax (m: xMax) =
        let l = m.location
        if gs.leftLight && l = LeftDoor then
            drawStillImage XLeftDoor rd (usualProgram rd)
        if gs.rightLight && l = RightDoor then
            drawStillImage XRightDoor rd (usualProgram rd)
            
    let inline drawScreamer (m: xMax) =
        match m.location with
        | SecurityOffice ->
            drawStillImage XStare rd (usualProgram rd)
        | _ -> ()
    
    let inline drawOffice () =
        drawStillImage OOffice rd (usualProgram rd)
        drawLight ()
        List.iter drawDoorMax gs.enemies
        drawDoor OLeftDoor1 OLeftDoor2 OLeftDoor3 gs.leftDoor
        drawDoor ORightDoor1 ORightDoor2 ORightDoor3 gs.rightDoor
        List.iter drawScreamer gs.enemies

    clear gs.time rd
    
    match gs.pov with
    | _, monitorStatus.InOffice -> drawOffice ()
    | location, _ -> drawRoom location

    