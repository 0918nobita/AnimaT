import commonjs from "@rollup/plugin-commonjs";
import resolve from "@rollup/plugin-node-resolve";
import serve from "rollup-plugin-serve";
import { terser } from "rollup-plugin-terser";

const production = !process.env.ROLLUP_WATCH;

export default {
  input: "./src/main.js",
  output: {
    format: "cjs",
    dir: "dist",
    sourcemap: true,
  },
  plugins: [
    commonjs(),
    resolve({
      browser: true,
      preferBuiltins: false,
    }),
    production && terser(),
    !production &&
      serve({
        open: true,
        openPage: "/",
        verbose: true,
        contentBase: "dist",
        host: "localhost",
        port: 1234,
        headers: {
          "Access-Control-Allow-Origin": "*",
        },
      }),
  ],
};
