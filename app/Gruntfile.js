module.exports = function(grunt) {

    grunt.initConfig({
        pkg: grunt.file.readJSON('package.json'),

        replace: {
            dev: {
                options: {
                    patterns: [
                        {
                            json: grunt.file.readJSON('./web/config/env/dev.json')
                        }, {
                            json: grunt.file.readJSON('./web/app/src/config/env/dev.json')
                        }
                    ],
                    verbose: true,
                    excludeBuiltins: false
                },
                files: [
                    {
                        expand: false,
                        flatten: true,
                        src: ['./web/config/env/Environment.tt.config.json'],
                        dest: './Web/config/Environment.config.json'
                    }, {
                        expand: false,
                        flatten: true,
                        src: ['./web/app/src/config/env/Const.tt.js'],
                        dest: './web/app/src/config/Const.js'
                    }
                ]
            }
        }
    });

    grunt.loadNpmTasks('grunt-replace');
};