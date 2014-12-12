module.exports = function(grunt) {

    grunt.initConfig({
        pkg: grunt.file.readJSON('package.json'),

        replace: {
            dev: {
                options: {
                    preserveOrder: true,
                    patterns: [
                        {
                            json: grunt.file.readJSON('./web/config/env/dev.json')
                        }, {
                            json: grunt.file.readJSON('./web/config/env/prod.json')
                        }, {
                            json: grunt.file.readJSON('./web/app/src/config/env/dev.json')
                        }, {
                            json: grunt.file.readJSON('./web/app/src/config/env/prod.json')
                        }, {
                            json: grunt.file.readJSON('./config.shared/dev.json')
                        }, {
                            json: grunt.file.readJSON('./config.shared/prod.json')
                        }
                    ]
                },
                files: [
                    {
                        expand: false,
                        flatten: true,
                        src: ['./web/config/env/Environment.config.json'],
                        dest: './web/config/Environment.config.json'
                    }, {
                        expand: false,
                        flatten: false,
                        src: ['./web/app/src/config/env/Config.js'],
                        dest: './web/app/src/config/Config.js'
                    }, {
                        expand: false,
                        flatten: false,
                        src: ['./api/config/env/Environment.config.json'],
                        dest: './api/config/Environment.config.json'
                    }, {
                        expand: false,
                        flatten: false,
                        src: ['./api/config/env/Auth.config.json'],
                        dest: './api/config/Auth.config.json'
                    }
                ]
            },
            prod: {
                options: {
                    patterns: [
                        {
                            json: grunt.file.readJSON('./web/config/env/prod.json')
                        }, {
                            json: grunt.file.readJSON('./web/app/src/config/env/prod.json')
                        }
                    ]
                },
                files: [
                    {
                        expand: false,
                        flatten: true,
                        src: ['./web/config/env/Environment.config.tt'],
                        dest: './web/config/Environment.config'
                    }, {
                        expand: false,
                        flatten: true,
                        src: ['./web/app/src/config/env/Config.js'],
                        dest: './web/app/src/config/Config.js'
                    }
                ]
            }
        },
        jshint: {
            options: {
                //reporter: 'jslint'
                //reporter: require('jshint-stylish')
                reporter: require('jshint-teamcity')
            },
            all: {
                options: {
                    '-W014': true,
                    '-W040': true,
                },
                src: ['web/app/src/**/*.js']
            }
        },
        less: {
            dev: {
                options: {
                    paths: ["web/app/less"]
                },
                files: { "web/app/less/all.css": "web/app/less/all.less" }
            },
            prod: {
                options: {
                    paths: ["web/app/less"],
                    cleancss: true
                },
                files: { "web/app/less/all.css": "web/app/less/all.less" }
            }
        },
        sprite: {
            32: {
                src: ['web/app/images/sprites/32/*.png'],
                destImg: 'web/app/images/sprites32.png',
                destCSS: 'web/app/less/sprites32.less',
                algorithm: 'alt-diagonal',
                cssFormat: 'less',
                imgOpts: {
                    format: 'png',
                    quality: 100
                },
            },
            64: {
                src: ['web/app/images/sprites/64/*.png'],
                destImg: 'web/app/images/sprites64.png',
                destCSS: 'web/app/less/sprites64.less',
                algorithm: 'alt-diagonal',
                cssFormat: 'less',
                imgOpts: {
                    format: 'png',
                    quality: 100
                },
            }
        }
    });

    grunt.loadNpmTasks('grunt-replace');
    grunt.loadNpmTasks('grunt-contrib-jshint');
    grunt.loadNpmTasks('grunt-contrib-less');
    grunt.loadNpmTasks('grunt-spritesmith');

    grunt.registerTask('transform', ['replace:dev', 'less:dev', 'sprite:32', 'sprite:64']);
};