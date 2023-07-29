module FiveNightsAtxMax_.Textures

open Browser.Types
open Fable.Core

type GL = WebGLRenderingContext

open Browser
open Fable.Core.JS
let imageLoadCanvas: HTMLCanvasElement = unbox document.createElement "canvas"
let imageLoadCanvasContext = imageLoadCanvas.getContext_2d()

let getImageData src k =
    let image: HTMLImageElement = unbox document.createElement "img"
    
    let onload _ =
        let width = image.width
        let height = image.height
        imageLoadCanvas.width <- width
        imageLoadCanvas.height <- height
        imageLoadCanvasContext.clearRect(0., 0., width, height)
        imageLoadCanvasContext.drawImage(U3.Case1(image), 0., 0., width, height)
        printfn "Loaded %s" src
        imageLoadCanvasContext.getImageData(0., 0., width, height) |> k
        
    image.onload <- onload
    image.crossOrigin <- "anonymous"
    image.src <- src

let loadTexture (gl : GL) url k =
    let texture = gl.createTexture ()
    gl.activeTexture gl.TEXTURE0
    gl.bindTexture (gl.TEXTURE_2D, texture)
    
    let level = 0;
    let internalFormat = gl.RGBA;
    let srcFormat = gl.RGBA;
    let srcType = gl.UNSIGNED_BYTE;
    
    getImageData url (fun data ->
    gl.bindTexture(gl.TEXTURE_2D, texture)
    gl.texImage2D(gl.TEXTURE_2D, level, internalFormat, srcFormat, srcType, data)
    gl.texParameteri(gl.TEXTURE_2D, gl.TEXTURE_WRAP_S, gl.CLAMP_TO_EDGE);
    gl.texParameteri(gl.TEXTURE_2D, gl.TEXTURE_WRAP_T, gl.CLAMP_TO_EDGE);
    gl.texParameteri(gl.TEXTURE_2D, gl.TEXTURE_MIN_FILTER, gl.LINEAR);
    
    k url texture)
    

let loadTextures (gl: GL) textures k =
    let d = Constructors.Map.Create ()
    
    gl.pixelStorei(gl.UNPACK_FLIP_Y_WEBGL, 1)
    
    let rec iLoadTextures (gl : GL) textures (d: JS.Map<string, WebGLTexture>) k =
        match textures with
        | [] -> k d
        | h :: tl ->
            loadTexture gl h (fun h tex ->
            let d = d.set (h, tex)
            iLoadTextures gl tl d k)
        
    iLoadTextures gl textures d k