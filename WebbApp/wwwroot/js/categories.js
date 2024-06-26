document.addEventListener('DOMContentLoaded', function () {
    select()
    searchQuery()
});




function select() {
    try {
        let select = document.querySelector(".select")
        let selected = document.querySelector(".selected")
        let selectOptions = document.querySelector(".select-options")


        selected.addEventListener('click', function () {
            selectOptions.style.display = (selectOptions.style.display === 'block') ? 'none' : 'block'
        })

        let options = selectOptions.querySelectorAll('.option')
        options.forEach(function (option) {
            option.addEventListener('click', function () {
                selected.innerHTML = this.textContent
                selectOptions.style.display = 'none'

                let category = this.getAttribute('data-value')
                selected.setAttribute('data-value', category)

                updateCoursesByFilters()
            })
        })

    }
    catch {

    }
}



function searchQuery() {
    try {
        document.querySelector("#searchQuery").addEventListener('keyup', function () {
            updateCoursesByFilters()
        })
    }
    catch {

    }
}



function updateCoursesByFilters() {
    const category = document.querySelector('.select .selected').getAttribute('data-value') || 'all'
    const searchQuery = document.querySelector("#searchQuery").value
    const url = `/courses/index?category=${encodeURIComponent(category)}&searchQuery=${encodeURIComponent(searchQuery)}`



    fetch(url)
        .then(res => res.text())
        .then(data => {
            const parser = new DOMParser()
            const dom = parser.parseFromString(data, 'text/html')
            document.querySelector('.course-items').innerHTML = dom.querySelector('.course-items').innerHTML


            const pagination = dom.querySelector('.pagination') ? dom.querySelector('.pagination').innerHTML : ''
            document.querySelector('.pagination').innerHTML = pagination

        })

}