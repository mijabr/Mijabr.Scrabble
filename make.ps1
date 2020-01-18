
$name = "scrabble"

Write-Host "Linux/ARM"
Write-Host "1. Build"
Write-Host "2. Push"
Write-Host "3. Build + Push"

Write-Host "windows/AMD86"
Write-Host "11. Build"
Write-Host "12. Push"
Write-Host "13. Build + Push"

$selection = Read-Host -Prompt "Choose"

switch ($selection)
{
    1 {docker buildx build -t mijabr/mijabr-${name}:arm -f Dockerfile.arm --platform linux/arm .}
    2 {docker push mijabr/mijabr-${name}:arm}
    3 {docker buildx build -t mijabr/mijabr-${name}:arm -f Dockerfile.arm --platform linux/arm .; docker push mijabr/mijabr-${name}:arm}

    11 {docker build -t mijabr/mijabr-${name}:amd86 .}
    12 {docker push mijabr/mijabr-${name}:amd86}
    13 {docker build -t mijabr/mijabr-${name}:amd86 .; docker push mijabr/mijabr-${name}:amd86}
}
