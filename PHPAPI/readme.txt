FragenGerangel API README

Dokumentation:

createUser.php
input:
	string username
	string password
output:
	void

getAuthToken.php
input:
	string username
	string password
output:
	string auth
beschreibung:
	Gibt ein Authentifizierungstoken zurück, welches für weitere API Funktionen verwendet wird.
	
// Für alle unterstehenden Funktionen wird ein Auth-Token benötigt, welches als Paramenter "auth" gesendet wird.

befriendUser.php
input:
	string user // der Benutzernamen des anderen Users
output:
	void
beschreibung:
	Sendet eine Freundesanfrage an einen anderen Benutzer. Falls dieser vorher bereits zurück angefragt hat, wird die Anfrage akzeptiert.
	
getFriendRequests.php
input:
	void
output:
	string[] usernames // Die Namen aller Benutzer, die Anfragen gesendet haben
	
getFriends.php
input:
	void
output:
	string[] usernames
	
startDuel.php
input:
	string username // der Benutzernamen des anderen Users, falls == "": Duell gegen Zufallsuser, der auch gerade sucht
output:
	void
beschreibung:
	Sendet eine Duellanfrage an einen anderen Benutzer. Falls dieser vorher bereits zurück angefragt hat, wird das Duell angenommen. Falls gegen einen Zufallsgegner gespielt werden soll, wird player_2 = null gesetzt
	
getDuelRequests.php
input:
	void
output:
	string[] usernames // die Namen aller Benutzer, die Anfragen gesendet haben
	
getDuelIDs.php
input:
	void
output:
	Duel[] games:
	[
		int gameID // id des Spiels
		string username // gegen wen gespielt wird
	]
beschreibung:
	gibt alle laufenden Spiele des Aufrufenden zurück

getRounds.php
input:
	int gameID
output:
	Round[] rounds:
	[
		int id
		int order
		string cat1 // die drei möglichen Kategorien
		string cat2
		string cat3
		int category // die gewählte Kategorie (1, 2 oder 3)
	]
	
chooseCategory.php
input:
	int gameID
	int category // (1, 2 oder 3; siehe getRounds.php)
output:
	void
beschreibung:
	Falls der Benutzer an der Reihe ist, eine Runde auszuwählen (== in der letzten Runde alle Fragen von beiden Spielern beantwortet wurden && der Spieler zu wählen hat (falls er Spieler 1 (Herausforderer) ist und die Rundenorder gerade ist oder er Spieler 2 ist und die Order ungerade ist))

getQuestionAnswers.php
input:
	int roundID
output:
	QuestionAnswer[] questionAnswers:
	[
		int id
		int order
		int questionID
		int yourAnswer
		int enemyAnswer
	]
	
getQuestion.php
input:
	int questionID
output:
	string category
	string question
	string correctAnswer
	string wrongAnswer1
	string wrongAnswer2
	string wrongAnswer3
	
uploadQuestionAnswer.php
input:
	int id // id des QuestionAnswer-Paares
	int answer // 0 = correctAnswer, 1 = wrongAnswer1 etc
output:
	[float deltaElo] // falls das Spiel mit dieser Antwort beendet wurde: die ELO-Punkte, die dazugewonnen bzw. verloren wurden
	
getStatistics.php
input:
	string username // die Statistiken dieses Benutzers werden zurückgegeben
output:
	int elo
	int perfectGames
	int wins
	int draws
	int losses
	Percentage[] categoryPercentages:
	[
		string category
		float percentage
	]
	
searchPlayer.php
input:
	string query
output
	string[] usernames