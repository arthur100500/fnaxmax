module CameraState

type monitorStatus =
    | InOffice
    | InCameras
    
type cameraLocation =
    | CamLeftCorridor
    | CamRightCorridor
    | CamBossOffice
    | CamEntryRoom
    | CamLeftHall
    | CamRightHall
    | CamLeftBackStage
    | CamRightBackStage
    | CamKitchen

type pointOfView = cameraLocation * monitorStatus

let moveLeft =
    function
    | CamLeftCorridor -> CamRightCorridor
    | CamRightCorridor -> CamBossOffice
    | CamBossOffice -> CamEntryRoom
    | CamEntryRoom -> CamLeftHall
    | CamLeftHall -> CamRightHall
    | CamRightHall -> CamLeftBackStage
    | CamLeftBackStage -> CamRightBackStage
    | CamRightBackStage -> CamKitchen
    | CamKitchen -> CamLeftCorridor
    
let moveRight =
    function
    | CamLeftCorridor -> CamKitchen
    | CamRightCorridor -> CamLeftCorridor
    | CamBossOffice -> CamRightCorridor
    | CamEntryRoom -> CamBossOffice
    | CamLeftHall -> CamEntryRoom
    | CamRightHall -> CamLeftHall
    | CamLeftBackStage -> CamRightHall
    | CamRightBackStage -> CamLeftBackStage
    | CamKitchen -> CamRightBackStage