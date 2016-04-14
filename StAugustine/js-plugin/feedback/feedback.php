<?php
header("Content-type: application/json");
$callback = 'getJsonResponse';
if (isset($_POST['callback']))
	$callback = $_POST['callback'];
function showerror($str)
{
	die($callback . '({"status": ' . mysql_errno() . ', "error": "in ' . $str . ' : ' . mysql_error() . '"});');
//	die("Error " . mysql_errno() . " in " . $str . ' : ' . mysql_error() . '.');
}
function mysqlclean($array, $index, $maxlength, $connection)
{
	if (isset($array["{$index}"]))
	{
		$input = substr($array["{$index}"], 0, $maxlength);
		$input = mysql_real_escape_string($input, $connection);
		return ($input);
	}
	return "";
}
if (! ($connection = mysql_pconnect("localhost", "cgmadmin", "cgmadmin")))
	die($callback . '({"status": -1000, "error": "Could not connect"});');
if (! mysql_select_db("cgm", $connection))
	showerror("select db");

$userFriendly = mysqlclean($_POST, "userFriendly", 45, $connection);
$improve = mysqlclean($_POST, "commentsUf", 1024, $connection);
$satisfaction = mysqlclean($_POST, "satisfied", 45, $connection);
$features = mysqlclean($_POST, "addtionalFun", 1024, $connection);
$coments = mysqlclean($_POST, "comments", 1024, $connection);
$subset = mysqlclean($_POST, "subset", 45, $connection);

$post_status="";

	$client_ip = mysqlclean($_SERVER, "REMOTE_ADDR", 16, $connection);
	$client_hostname = gethostbyaddr($client_ip);
	$query = "insert into cgmfeedback (clientIP, clientHostname";
	$values = ") values ('" . $client_ip . "','" . $client_hostname . "'";
	
	if ($userFriendly != "" && is_numeric($userFriendly))
	{
		$query .= ", userFriendly";
		$values .= "," . $userFriendly;
	}
	if ($satisfaction != "" && is_numeric($satisfaction))
	{
		$query .= ", satisfied";
		$values .= "," . $satisfaction;
	}
	if ($improve != "")
	{
		$query .= ", commentsUf";
		$values .= ",'" . $improve . "'";
	}
	if ($features != "")
	{
		$query .= ", addtionalFun";
		$values .= ",'" . $features . "'";
	}
	if ($coments != "")
	{
		$query .= ", comments";
		$values .= ",'" . $coments . "'";
	}
	if ($subset != "")
	{
		$query .= ", subset";
		$values .= ",'" . $subset . "'";
	}
	if (! mysql_query($query . $values . ")", $connection))
		showerror($query . $values);
	//echo $callback, '({"status": 0});';
	echo '{"status": 0}';
?>
