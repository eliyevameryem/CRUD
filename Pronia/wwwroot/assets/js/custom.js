const addbasketbtns = document.querySelectorAll(".add-basket-button");
const box = document.querySelector(".basket-books");

addbasketbtns.forEach(btn => {
btn.addEventListener("click", function(e) {
    e.preventDefault();

    var endpoint = btn.getAttribute("href");

    fetch(endpoint)
        .then(respose => respose.text())
        .then(data => {
            box.innerHTML = data;
        })
    })
})