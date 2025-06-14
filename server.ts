Bun.serve({
  fetch(req, res) {
    console.log(req);
    console.log(req.text());
  },
  port: 4000,
});

console.info("Server started");
