


function saveCourse(id){

    fetch(`/account/savedcourses?id=${id}`)
        .then(res => {
            if (res.ok)
                window.location.reload();
            else
                console.log("something went wrong :(")
        })
}


function saveCourseDelete(id) {
    fetch(`/account/savedcoursescontentdelete/${id}`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        // You can include a request body if needed
        // body: JSON.stringify({ id: id })
    })
        .then(res => {
            if (res.ok) {
                // Reload the page if the request is successful
                window.location.reload();
            } else {
                console.log("Something went wrong :(");
            }
        })
        .catch(error => {
            console.error('Error:', error);
        });
}


function saveCourseDeleteAll() {
    fetch(`/account/savedcoursescontentdeleteall`, {
        method: 'POST'
    })
        .then(res => {
            if (res.ok) {
                // Reload the page if the request is successful
                window.location.reload();
            } else {
                console.log("Something went wrong :(");
            }
        })
        .catch(error => {
            console.error('Error:', error);
        });
}
