/*
function saveCourse(courseId) {
    var returnUrl = window.location.pathname;
    fetch(`/Account/SaveCourse?id=${courseId}&returnUrl=${encodeURIComponent(returnUrl)}`)
        .then(response => response.text())
        .then(data => {
            window.location.reload();
        })
        .catch(error = console.error('Error:', error))
}


*/




function saveCourse(id){

    fetch(`/account/savedcourses?id=${id}`)
        .then(res => {
            if (res.ok)
                window.location.reload();
            else
                console.log("something went wrong :(")
        })

  
}