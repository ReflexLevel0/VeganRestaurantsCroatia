class CommandHelper{
    async executeProcess(command){
        const { exec } = require('child_process');

        exec(command, (error, stdout, stderr) => {
            if (error) {
                console.error(`Error executing command: ${error.message}`);
                return;
            }

            if (stderr) {
                console.error(`Command stderr: ${stderr}`);
                return;
            }

            console.log(`Command output: ${stdout}`);
        });
    }
}

module.exports = CommandHelper