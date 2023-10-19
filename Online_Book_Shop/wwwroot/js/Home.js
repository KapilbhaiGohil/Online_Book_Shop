function GiveBook(book){
    return `
        <div class="book">
                <div class="book-img">
                    <img src="/Images/BookImages/${book.imageFileName}"/>
                </div>
                <div class="book-info">
                    <div class="date">
                        <img src="/Images//icons8-date-96.png" />
                        <span>${book.publish.substr(0,10)}</span>
                    </div>
                    <div class="book-name">
                        <h3> ${book.name.length <= 10 ? book.name : book.name.substr(0, 10) + '...'}</h3>
                    </div>
                    <div class="book-price">
                        ${book.price.toLocaleString("en-IN", {
                            style: "currency",
                            currency: "INR"
                        })}
                    </div>
                    <div class="desc">
                        ${book.description.length <= 40 ?book.description:book.description.substr(0, 40)+'...'}
                    </div>
                    <div class="rating">
                        <img src="/Images/icons8-star-filled-96.png" />
                        <img src="/Images/icons8-star-filled-96.png" />
                        <img src="/Images/icons8-star-filled-96.png" />
                        <img src="/Images/icons8-star-filled-96.png" />
                        <img src="/Images/icons8-star-half-empty-90.png" />
                    </div>
                    <div class="cart-btn">
                        <input type="button" value="View Info"/>
                    </div>
                </div>
            </div>
    `
}
let content = ``;
let books = document.getElementById("books");
for (let i = 0; i < bookList.length && i<5; i++) {
    content += GiveBook(bookList[i])
}
books.innerHTML = content;