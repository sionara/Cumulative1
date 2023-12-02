function AddTeacher() {

	// URL to direct the API call.
	var URL = "http://localhost:54597/api/TeacherData/AddTeacher"; //not working. something is incorrect with this URL.

	var xhr = new XMLHttpRequest();


	var TeacherFname = document.getElementById('TeacherFname').value;
	var TeacherLname = document.getElementById('TeacherLname').value;
	var EmployeeNumber = document.getElementById('eNum').value;
	var HireDate = document.getElementById('HireDate').value;
	var Salary= document.getElementById('Salary').value;


	var TeacherData = {
		"TeacherFname": TeacherFname,
		"TeacherLname": TeacherLname,
		"EmployeeNumber": EmployeeNumber,
		"HireDate": HireDate,
		"Salary": Salary
	};


	xhr.open("POST", URL, true);
	xhr.setRequestHeader("Content-Type", "application/json");
	xhr.onreadystatechange = function () {
		//ready state should be 4 AND status should be 200
		if (xhr.readyState == 4 && xhr.status == 200) {

			//nothing to put here since we are not injecting new HTML from the client-side.
		}

	}
	//POST information sent through the .send() method
	xhr.send(JSON.stringify(TeacherData)); //WE get a 400 error here.

}