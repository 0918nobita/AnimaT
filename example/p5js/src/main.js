import p5 from "p5";

const sketch = (p) => {
  p.setup = () => {
    p.createCanvas(800, 600);
    p.background(0);
  };

  p.draw = () => {
    p.fill(p.mouseIsPressed ? 0 : 255);
    p.ellipse(p.mouseX, p.mouseY, 80, 80);
  };
};

window.addEventListener("DOMContentLoaded", () => {
  new p5(sketch, document.getElementById("container"));
});
