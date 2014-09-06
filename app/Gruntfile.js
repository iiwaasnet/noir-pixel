module.exports = function (grunt) {

    grunt.initConfig({
        pkg: grunt.file.readJSON('package.json'),

        replace: {
            dev: {
                options: {
                    patterns: [{
                        json: grunt.file.readJSON('./web/config/env/dev.json')
                    }],
                    excludeBuiltins: false
                },
                files: [{
                    expand: true,
                    flatten: true,
                    src: ['./web/config/env/Environment.config.json'],
                    dest: './Web/config/'
                },{
                    expand: true,
                    flatten: true,
                    src: ['./web/app/src/config/env/Const.js'],
                    dest: './web/app/src/config/'
                }]
            }
        }
    });

    grunt.loadNpmTasks('grunt-replace');
};