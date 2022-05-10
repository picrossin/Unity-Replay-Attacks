<?php
$file = "score.txt";
if (!file_exists($file)) {
    touch($file);
}

$handle = fopen($file, "r+");

if(isset($_POST["unitypost"])){
    $scoreText = fread($handle, filesize($file));
    $delimiter = ' ';
    $scores = explode($delimiter, $scoreText);
    
    $leftScored = $_POST["unitypost"] == "left";
    
    $newLeftScore = intval($scores[0]);
    $newRightScore = intval($scores[1]);

    if ($leftScored) {
        $newLeftScore = $newLeftScore + 1;
    } else {
        $newRightScore = $newRightScore + 1;
    }

    $score = $newLeftScore . " " . $newRightScore;

    ftruncate($handle, 0);
    rewind($handle);
    fwrite($handle, $score);
    flock($handle, LOCK_UN);
    echo "Successfully wrote message " . $score . " to the file.";
}else if(isset($_GET["unityget"])) {
    $scoreText = fread($handle, filesize($file));
    echo $scoreText;
}

fclose($handle);
?>