module.exports = {
    "/api": {
        target:
            process.env["services__WebApplication1__http__0"] ||
            process.env["services__WebApplication1__https__0"],
        secure: process.env["NODE_ENV"] !== "development",
        pathRewrite: {
            "^/api": "",
        }
    },
};
