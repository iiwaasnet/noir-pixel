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
                            json: grunt.file.readJSON('./web/app/src/config/env/dev.json')
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
                        dest: './Web/config/Environment.config'
                    }, {
                        expand: false,
                        flatten: false,
                        src: ['./web/app/src/config/env/Const.tt'],
                        dest: './web/app/src/config/Const.js'
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
                        dest: './Web/config/Environment.config'
                    }, {
                        expand: false,
                        flatten: true,
                        src: ['./web/app/src/config/env/Const.tt'],
                        dest: './web/app/src/config/Const.js'
                    }
                ]
            }
        }
    });

    grunt.loadNpmTasks('grunt-replace');
};