const path = require('path');
const HtmlWebpackPlugin = require('html-webpack-plugin');

// Host
const host = process.env.HOST || 'localhost';


module.exports = {

    // Building mode
    mode: 'development',

    // Entry point of the application
    entry: './src/main.ts',

    output: {
        // Target application
        path: path.resolve(__dirname, 'dist'),
        filename: 'bundle.js'
    },

    module: {
        rules: [
            {
                test: /\.(ts|tsx)$/,
                exclude: /node_modules/,
                loader: 'ts-loader'
            }
        ]
    },

    plugins: [
        // Re-generate index.html with injected script tag.
        new HtmlWebpackPlugin({
            inject: true,
            template: 'dist/index.html'
        })
    ],

    devServer: {

        // Serve index.html as the base
        contentBase: path.resolve(__dirname, 'dist'),


        // Enable compression
        compress: true,

        // Enable hot reloading
        hot: true,
        host,
        port: 8000,

        // Public path is root of content base
        publicPath: '/',

        proxy: {
            '/api': {
                target: "http://localhost:8000",
                bypass: (req, res) => res.send({
                    mssg: 'proxy server <from bypass in webpack>'
                })
            }
        }
    }
};