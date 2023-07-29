module FiveNightsAtxMax_.Shaders

let vertexShader =
    """
    precision mediump float;
    attribute vec2 aPosition;
    attribute vec2 aTexCoord;

    varying vec2 texCoord;

    void main() {
        gl_Position = vec4(aPosition, 0.0, 1.0);
        texCoord = aTexCoord;
    }"""

let fragmentShader =
    """
    precision mediump float;
    varying vec2 texCoord;
    uniform sampler2D img0;

    void main(){
        vec4 color = texture2D(img0, texCoord);
        gl_FragColor = color;
    }"""
    
let staticFragmentShaders =
    """
    precision mediump float;
    varying vec2 texCoord;
    uniform sampler2D img0;
    uniform float time;
    
    float maxStrength = 0.5;
    float minStrength = 0.125;
    float speed = 10.00;

    float random(vec2 noise){
        return fract(sin(dot(noise.xy,vec2(10.998,98.233))) * 12433.14159265359);
    }

    void main(){
        vec2 uv = texCoord.xy;
        vec2 uv2 = fract(texCoord.xy * fract(sin(time * speed)));
        maxStrength = 0.1;
        vec3 colour = vec3(random(uv2.xy)) * maxStrength;
        vec3 background = vec3(texture2D(img0, uv));
        
        gl_FragColor = vec4(background + colour, 1.0);
    }"""