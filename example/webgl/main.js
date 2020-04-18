const canvas = document.getElementById("screen");
const gl = canvas.getContext("webgl");

const loadShader = (type, source) => {
  const shader = gl.createShader(type);
  gl.shaderSource(shader, source);
  gl.compileShader(shader);
  if (!gl.getShaderParameter(shader, gl.COMPILE_STATUS)) {
    throw new Error(gl.getShaderInfoLog(shader));
  }
  return shader;
};

// 頂点シェーダのソースコード
const vsSource = `
  attribute vec3 position;
  attribute vec3 aColor;

  varying lowp vec4 vColor;

  void main() {
    gl_Position = vec4(position, 1.0);
    vColor = vec4(aColor, 1.0);
  }
`;

// フラグメントシェーダのソースコード
const fsSource = `
  varying lowp vec4 vColor;

  void main() {
    gl_FragColor = vColor;
  }
`;

const vertexShader = loadShader(gl.VERTEX_SHADER, vsSource);
const fragmentShader = loadShader(gl.FRAGMENT_SHADER, fsSource);

const shaderProgram = gl.createProgram();
gl.attachShader(shaderProgram, vertexShader);
gl.attachShader(shaderProgram, fragmentShader);
gl.linkProgram(shaderProgram);

if (!gl.getProgramParameter(shaderProgram, gl.LINK_STATUS)) {
  throw new Error(gl.getProgramInfoLog(shaderProgram));
}

const programInfo = {
  attribLoc: {
    position: gl.getAttribLocation(shaderProgram, "position"),
    color: gl.getAttribLocation(shaderProgram, "aColor"),
  },
};

const posBuf = gl.createBuffer();

// prettier-ignore
const pos = [
  -0.333, 0.5,
  0.333, 0.5,
  -0.333, -0.5,
  0.333, -0.5,
];

gl.bindBuffer(gl.ARRAY_BUFFER, posBuf);
gl.bufferData(gl.ARRAY_BUFFER, new Float32Array(pos), gl.STATIC_DRAW);

// prettier-ignore
gl.vertexAttribPointer(
  programInfo.attribLoc.position,
  /* numComponents */ 2,
  /* type */ gl.FLOAT,
  /* normalize? */ false,
  /* stribe */ 0,
  /* offset */ 0
);

gl.enableVertexAttribArray(programInfo.attribLoc.position);

const colorBuf = gl.createBuffer();

// prettier-ignore
const color = [
  255, 255, 255,
  255,   0,   0,
    0, 255,   0,
    0,   0, 255,
].map(n => n / 255);

gl.bindBuffer(gl.ARRAY_BUFFER, colorBuf);
gl.bufferData(gl.ARRAY_BUFFER, new Float32Array(color), gl.STATIC_DRAW);

// prettier-ignore
gl.vertexAttribPointer(
  programInfo.attribLoc.color,
  /* numComponents */ 3,
  /* type */ gl.FLOAT,
  /* normalize? */ false,
  /* stribe */ 0,
  /* offset */ 0
);

gl.enableVertexAttribArray(programInfo.attribLoc.color);

gl.useProgram(shaderProgram);

gl.clearColor(0.0, 0.0, 0.0, 1.0);
gl.clearDepth(1.0);
gl.enable(gl.DEPTH_TEST);
gl.depthFunc(gl.LEQUAL);
gl.clear(gl.COLOR_BUFFER_BIT | gl.DEPTH_BUFFER_BIT);

gl.drawArrays(gl.TRIANGLE_STRIP, 0, 4);

gl.flush();
