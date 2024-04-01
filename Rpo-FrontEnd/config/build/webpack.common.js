const webpack = require('webpack');
const FaviconsWebpackPlugin = require('favicons-webpack-plugin');
const HtmlWebpackPlugin = require('html-webpack-plugin');
const helpers = require('../helpers');
const ExtractTextPlugin = require('extract-text-webpack-plugin');
const GoogleFontsPlugin = require("google-fonts-webpack-plugin");


module.exports = {
    entry: {
        'polyfills': './src/polyfills.ts',
        'vendor': './src/vendor.ts',
        'app': './src/main.ts',
        'styles': './src/global.scss'
    },

    resolve: {
        extensions: [
            '.js', '.ts'
        ],
        alias: {
            'blank-avatar': helpers.root('src', 'app', 'assets', 'blank-avatar.jpg')
        }
    },

    module: {
        rules: [{
                test: /\.ts$/,
                use: ['awesome-typescript-loader?configFileName=config/tsconfig.json', 'angular2-template-loader']
            },
            {
                test: /\.html$/,
                use: 'html-loader'
            },
            {
                test: /\.scss$/,
                exclude: [/node_modules/, helpers.root('src', 'global.scss')],
                use: ['to-string-loader', 'css-loader', 'sass-loader']
            },
            {
                test: /bootstrap\/dist\/js\//,
                use: 'imports-loader?jQuery=jquery'
            },
            {
                test: /global\.scss$/,
                use: ExtractTextPlugin.extract({
                    use: 'css-loader!sass-loader'
                })
            },
            {
                test: /\.(png|jpe?g|gif|svg|woff|woff2|otf|ttf|eot|ico)$/,
                exclude: [/font-awesome/, /glyphicons/],
                use: 'file-loader?name=assets/[name].[hash].[ext]'
            },
            {
                test: /\.css$/,
                loader: 'style-loader!css-loader'
            },
            {
                test: /\.woff(2)?(\?v=[0-9]\.[0-9]\.[0-9])?$/,
                use: {
                    loader: 'url-loader',
                    options: {
                        limit: 10000,
                        mimetype: 'application/font-woff'
                    }
                }
            },
            {
                test: /\.(ttf|eot|svg)(\?v=[0-9]\.[0-9]\.[0-9])?$/,
                loader: 'file-loader'
            },
            {
                test: require.resolve('jquery'),
                use: [{
                    loader: 'expose-loader',
                    options: 'jQuery'
                }]
            }
        ]
    },

    plugins: [
        new webpack.ProvidePlugin({
            $: 'jquery',
            jQuery: 'jquery',
            objectFitImages: 'object-fit-images'
        }),

        new webpack.optimize.CommonsChunkPlugin({
            name: ['app', 'vendor', 'polyfills']
        }),

        new FaviconsWebpackPlugin('./src/logo-icon.png'),

        new HtmlWebpackPlugin({
            template: 'src/index.html'
        }),

        new webpack.ContextReplacementPlugin(
            /angular(\\|\/)core(\\|\/)@angular/,
            helpers.root('src'), {}
        ),

        new GoogleFontsPlugin({
            fonts: [{
                    family: "PT Sans"
                }, {
                    family: "Open Sans"
                } // ,{ family: "Roboto", variants: [ "400", "700italic" ] }
            ],
            local: false
        })
    ]
};