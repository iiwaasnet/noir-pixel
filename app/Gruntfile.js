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
                src: ['web/app/src/**/*.js', 'web/app/src/*.js']
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
            16: {
                src: ['../graphics/src/16/*.png'],
                destImg: 'web/app/images/sprites16.png',
                destCSS: 'web/app/less/sprites16.less',
                algorithm: 'alt-diagonal',
                cssFormat: 'less',
                imgOpts: {
                    format: 'png',
                    quality: 100
                },
            },
            32: {
                src: ['../graphics/src/32/*.png'],
                destImg: 'web/app/images/sprites32.png',
                destCSS: 'web/app/less/sprites32.less',
                algorithm: 'alt-diagonal',
                cssFormat: 'less',
                imgOpts: {
                    format: 'png',
                    quality: 100
                },
            },
            login: {
                src: ['../graphics/src/login/*.png'],
                destImg: 'web/app/images/sprites-login.png',
                destCSS: 'web/app/less/sprites-login.less',
                algorithm: 'alt-diagonal',
                cssFormat: 'less',
                imgOpts: {
                    format: 'png',
                    quality: 100
                },
            }
        },
        concat: {
            all: {
                files: {
                    'web/app/src/app.js': [
                        'web/app/src/**/**/*.module.js',
                        'web/app/src/!(config)**/**/!(*.module.js)*.js'
                    ],
                    'web/app/src/ng.min.js': [
                        'web/app/vendor/angular.min.js',
                        'web/app/vendor/angular-animate.min.js',
                        'web/app/vendor/angular-messages.min.js'
                    ],
                    'web/app/src/vendor.js': [
                        'web/app/vendor/stacktrace.js',
                        'web/app/vendor/nprogress.js'
                    ],
                    'web/app/src/vendor-ng.min.js': [
                        'web/app/vendor/ngDialog.min.js',
                        'web/app/vendor/angular-webstorage.min.js',
                        'web/app/vendor/angular-ui-router.min.js'
                    ]
                }
            },
            nativeMin: {
                files: {
                    'web/app/src/vendor-native.min.js': [
                        'web/app/vendor/moment.min.js',
                        'web/app/src/vendor-native.tmp.js'
                    ]
                }
            }
        },
        uglify: {
            options: {
                mangle: true,
                compress: {
                    global_defs: {
                        "DEBUG": false
                    },
                    drop_console: true
                }
            },
            all: {
                files: {
                    'web/app/src/vendor-native.tmp.js': [
                        'web/app/vendor/stacktrace.js',
                        'web/app/vendor/nprogress.js'
                    ],
                    'web/app/src/app.min.js': 'web/app/src/app.js'
                }
            }
        }
    });
    grunt.loadNpmTasks('grunt-replace');
    grunt.loadNpmTasks('grunt-contrib-jshint');
    grunt.loadNpmTasks('grunt-contrib-less');
    grunt.loadNpmTasks('grunt-spritesmith');
    grunt.loadNpmTasks('grunt-contrib-concat');
    grunt.loadNpmTasks('grunt-contrib-uglify');

    grunt.registerTask('transform', [
        'replace:dev',
        'sprite:16',
        'sprite:32',
        'sprite:login',
        'less:dev',
        'concat:all',
        'uglify:all',
        'concat:nativeMin'
    ]);
};