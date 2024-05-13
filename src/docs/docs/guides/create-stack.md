### Prerequisites
Before you proceed, make sure you have completed the installation of ReStack as outlined in the [Installation Guide](guides/installation).

### Step 1: Accessing ReStack
1. Open your web browser and navigate to the ReStack application.

### Step 2: Creating a stack
1. Click on the "Add Stack" button.
2. Fill in the required details for your stack:
    - **Name**: Provide a name for your stack (e.g., "Example Stack").
    - **Run as**: Select the type of stack you want to create.
    - **Fail on std error output**: Check if the stack should fail, the moment input is received on the std error output.
    - **Libraries**: Optionally, select the libraries you want to use.
4. Click on the "Save" button to create your stack.

### Step 3: Adding the code
1. After creating the stack, you'll be redirected to the stack code editor.
2. Here you can add the code you want to execute.
3. Click on the "Save" button to save the code.

### Step 4: Executing a stack
Just click the 'Execute' button and view your output!

### Example Stack (for Testing)
Here's an example stack configuration to get you started:

### Windows (bat)

This script will output 10 lines with random text.

```bat
@echo off
setlocal enabledelayedexpansion

set count=0
:loop
set /a count+=1
if !count! gtr 10 goto :eof

set "chars=abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789"
set "length=100"
set "randomString="

for /l %%i in (1,1,%length%) do (
    set /a "rand=!random! %% 62"
    for %%a in (!rand!) do (
        set "randomString=!randomString!!chars:~%%a,1!"
    )
)

echo %randomString%

goto :loop
```

### Linux (shell)

This script will make an SSH connection to the specified IP address. On the server, it will execute lxc-attach command to update a running container.

```shell
VM_ID=100
SSH_USER=root
SSH_HOST_IP=192.168.1.1
SSH_KEY=/restack/data/keys/id_rsa
DOCKER_COMPOSE_FILE=~/vcode/workspace/stream/docker-compose.yml

ssh ${SSH_USER}@${SSH_HOST_IP} -i ${SSH_KEY} -o StrictHostKeyChecking=no "lxc-attach ${VM_ID} -- apt-get update"
ssh ${SSH_USER}@${SSH_HOST_IP} -i ${SSH_KEY} -o StrictHostKeyChecking=no "lxc-attach ${VM_ID} -- apt-get upgrade -y"
ssh ${SSH_USER}@${SSH_HOST_IP} -i ${SSH_KEY} -o StrictHostKeyChecking=no "lxc-attach ${VM_ID} -- apt-get dist-upgrade -y"
ssh ${SSH_USER}@${SSH_HOST_IP} -i ${SSH_KEY} -o StrictHostKeyChecking=no "lxc-attach ${VM_ID} -- docker compose -f ${DOCKER_COMPOSE_FILE} pull"
ssh ${SSH_USER}@${SSH_HOST_IP} -i ${SSH_KEY} -o StrictHostKeyChecking=no "lxc-attach ${VM_ID} -- docker compose -f ${DOCKER_COMPOSE_FILE} up -d"
ssh ${SSH_USER}@${SSH_HOST_IP} -i ${SSH_KEY} -o StrictHostKeyChecking=no "lxc-attach ${VM_ID} -- docker ps"
```

Feel free to customize the example stack configuration according to your specific needs and requirements.
