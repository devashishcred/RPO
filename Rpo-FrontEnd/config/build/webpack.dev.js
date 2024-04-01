const webpackMerge = require('webpack-merge');
const webpack = require('webpack');
const commonConfig = require('./webpack.common.js');
const helpers = require('../helpers');
const ExtractTextPlugin = require('extract-text-webpack-plugin');
const BundleAnalyzerPlugin = require('webpack-bundle-analyzer').BundleAnalyzerPlugin;

module.exports = webpackMerge(commonConfig, {
    devtool: 'cheap-module-eval-source-map',

    output: {
        path: helpers.root('dist'),
        publicPath: 'http://localhost:3000/',
        filename: '[name].js',
        chunkFilename: '[id].chunk.js'
    },

    devServer: {
        historyApiFallback: true,
        stats: 'minimal'
        /*,
                TODO setup when REST service ready
                proxy: {
                    '/api/**': {
                        target: 'http://localhost:8080/your-rest-service',
                        secure: false,
                        changeOrigin: true
                    }
                }*/
    },

    plugins: [
        new ExtractTextPlugin('styles.css'),

        new BundleAnalyzerPlugin(),
        new webpack.ContextReplacementPlugin(
            // The (\\|\/) piece accounts for path separators in *nix and Windows
          
            // For Angular 5, see also https://github.com/angular/angular/issues/20357#issuecomment-343683491
            /\@angular(\\|\/)core(\\|\/)esm5/,
            helpers.root('src'), // location of your src
            {
              // your Angular Async Route paths relative to this root directory
            }
          ),
    ]
});