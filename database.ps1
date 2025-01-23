# Use this script to start a docker container for a local development database
# Requires Docker Desktop for Windows - https://docs.docker.com/docker-for-windows/install/

$DB_CONTAINER_NAME = "logonya-postgres"

# Check if Docker is installed
if (-not (Get-Command docker -ErrorAction SilentlyContinue)) {
    Write-Error "Docker is not installed. Please install docker and try again.`nDocker install guide: https://docs.docker.com/engine/install/"
    exit 1
}

# Check if Docker daemon is running
try {
    docker info | Out-Null
} catch {
    Write-Error "Docker daemon is not running. Please start Docker and try again."
    exit 1
}

# Check if container is already running
if (docker ps -q -f name=$DB_CONTAINER_NAME) {
    Write-Host "Database container '$DB_CONTAINER_NAME' already running"
    exit 0
}

# Check if container exists but is stopped
if (docker ps -q -a -f name=$DB_CONTAINER_NAME) {
    docker start $DB_CONTAINER_NAME
    Write-Host "Existing database container '$DB_CONTAINER_NAME' started"
    exit 0
}

# Import env variables from .env
$envContent = Get-Content .env
foreach ($line in $envContent) {
    if ($line -match '^([^=]+)=(.*)$') {
        Set-Item -Path "Env:$($matches[1])" -Value $matches[2]
    }
}

# Parse DATABASE_URL for password and port
$DB_PASSWORD = $env:DB_PASSWORD
$DB_PORT = $env:DB_PORT
$DB_HOST = $env:DB_HOST
$DB_USER = $env:DB_USER
$DB_DATABASE = $env:DB_DATABASE

if ($DB_PASSWORD -eq "password") {
    Write-Host "You are using the default database password"
    $REPLY = Read-Host "Should we generate a random password for you? [y/N]"
    if ($REPLY -notmatch '^[Yy]$') {
        Write-Host "Please change the default password in the .env file and try again"
        exit 1
    }
    # Generate a random URL-safe password
    $DB_PASSWORD = [Convert]::ToBase64String([System.Security.Cryptography.RandomNumberGenerator]::GetBytes(9)) -replace '\+','-' -replace '/','_'
    (Get-Content .env) -replace ':password@', ":$DB_PASSWORD@" | Set-Content .env
}

# Start the container
docker run -d `
    --name $DB_CONTAINER_NAME `
    -e POSTGRES_USER=$DB_USER `
    -e POSTGRES_PASSWORD="$DB_PASSWORD" `
    -e POSTGRES_DB=$DB_DATABASE `
    -p "${DB_PORT}:5432" `
    docker.io/postgres

if ($?) {
    Write-Host "Database container '$DB_CONTAINER_NAME' was successfully created"
}