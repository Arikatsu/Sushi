#!/bin/bash

PROJECT_DIR="/home/ubuntu/Sushi-v3"
LOG_FILE="$PROJECT_DIR/deploy.log"
echo "$(date): Starting deployment" >> $LOG_FILE

cd "$PROJECT_DIR" || { echo "$(date): Failed to change directory to $PROJECT_DIR" >> $LOG_FILE; exit 1; }

git pull >> $LOG_FILE 2>&1

if [ $? -eq 0 ]; then
    echo "$(date): Git pull successful" >> $LOG_FILE
    
    echo "$(date): Stopping existing app" >> $LOG_FILE
    screen -S sushi -X quit 2>/dev/null || true
    
    echo "$(date): Building .NET application" >> $LOG_FILE
    dotnet build Sushi.csproj --configuration Release >> $LOG_FILE 2>&1
    
    if [ $? -eq 0 ]; then
        echo "$(date): Build successful, starting application" >> $LOG_FILE

        BUILD_PATH="$PROJECT_DIR/bin/Release/net6.0"
        
        if [ ! -f "$BUILD_PATH/Sushi" ]; then
            echo "$(date): ERROR - Executable not found at $BUILD_PATH" >> $LOG_FILE
            exit 1
        fi

        screen -dmS sushi bash -c "cd $BUILD_PATH && ./Sushi"

        sleep 3
        
        if screen -list | grep -q "sushi"; then
            echo "$(date): Application started successfully in screen session 'app'" >> $LOG_FILE
        else
            echo "$(date): Application failed to start" >> $LOG_FILE
            exit 1
        fi
    else
        echo "$(date): Build failed" >> $LOG_FILE
        exit 1
    fi
    
    echo "$(date): Deployment completed successfully" >> $LOG_FILE
else
    echo "$(date): Git pull failed" >> $LOG_FILE
    exit 1
fi