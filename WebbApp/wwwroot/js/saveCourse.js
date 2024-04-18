
function saveCourse(courseId) {
    // Make an AJAX request to the SaveCourse action
    console.log(courseId)
    $.ajax({
        type: "POST",
        url: "/Courses/SaveCourse",
        data: { id: courseId },
        success: function (response) {
            // Handle success response here
            console.log("Course saved successfully");
        },
        error: function (xhr, status, error) {
            // Handle error response here
            console.error("Error saving course: " + error);
        }
    });
}


