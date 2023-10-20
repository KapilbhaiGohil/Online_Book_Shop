let bookRows = [];
let uniqueId = 0;

GiveBookRow(uniqueId, "Popular Books of the week", bookList)

function GiveBookRow(id, heading, BookList) {
    let Slides = GenerateSlides(BookList);
    let RowContent = `
         <div id="book-row${id}" class="book-row">
             <div class="heading">
                 <h1>${heading}</h1>
             </div>
             <div id="books${id}" class="books">
                `+ Slides[0] + GiveBtnContent(id) +`
                
             </div>
         </div>
    
    `;
    document.getElementById("home-books").innerHTML += RowContent;
    bookRows.push({ uniqueId: id, currentShowId: 0, BookSlides: Slides });
    uniqueId = (uniqueId + 1) % 1000;
    console.log(bookRows);
}
function GiveBtnContent(id) {
    return `
    <div  onclick="forClick(${id})"  id="for-arr${id}" class="forward-arr" >
                    <img src="/Images/icons8-forward-96.png" />
                </div >
                <div onclick="backClick(${id})" style="display:none" id="back-arr${id}" class="back-arr">
                    <img src="/Images/icons8-back-96.png" />
                </div>
                `
}
function GenerateSlides(List) {
    let res = [];
    for (let i = 0; i < List.length; i++) {
        let resarr = ``;
        let j = i;
        for (j = i; j < List.length && j < i + 5; j++) {
            resarr += GiveBook(List[j]);
        }
        res.push(resarr);
        i = j-1;
    }
    return res;
}
function GiveBookContent(BookList) {
    let bookContent = ``;
    for (let i = 0; i < BookList.length && i < 5; i++) {
        bookContent += GiveBook(BookList[i]);
    }
    return bookContent;
}
function GiveBook(book) {
    let stringifiedObj = JSON.stringify(book);
    return `
        <div id="book" class="book">
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
                    <div  class="cart-btn">
                        <input onclick="ShowFullInfo('${encodeURIComponent(stringifiedObj)}')" type="button" value="View Info"/>
                    </div>
                </div>
            </div>
    `
}


function backClick(id) {
    let rowIndex = bookRows.findIndex((row) => row.uniqueId == id)
    let row = bookRows[rowIndex];
    let currIndex = row.currentShowId;
    let booksEle = document.getElementById(`books${id}`);
    if (currIndex - 1 >= 0) {
        currIndex -= 1;
        booksEle.style.opacity = 0
        setTimeout(() => {
            booksEle.style.opacity = 1;
            booksEle.innerHTML = row.BookSlides[currIndex] + GiveBtnContent(id);
            AdjustBtnDisplay(`back-arr${id}`, `for-arr${id}`, currIndex, row.BookSlides.length);
        }, 300);
    }
    row.currentShowId = currIndex;
    bookRows[rowIndex] = row;
}
function forClick(id) {

    let rowIndex = bookRows.findIndex((row) => row.uniqueId == id)
    let row = bookRows[rowIndex];
    let currIndex = row.currentShowId;
    let booksEle = document.getElementById(`books${id}`);
    if (currIndex + 1 < row.BookSlides.length) {
        currIndex += 1;
        booksEle.style.opacity = 0
        setTimeout(() => {
            booksEle.style.opacity = 1;
            booksEle.innerHTML = row.BookSlides[currIndex] + GiveBtnContent(id);
            AdjustBtnDisplay(`back-arr${id}`, `for-arr${id}`, currIndex, row.BookSlides.length);
        }, 300);
    }
    row.currentShowId = currIndex;
    bookRows[rowIndex] = row;
}
function AdjustBtnDisplay(backBtn, rightBtn,currIndex,MaxIndex) {
    let left = document.getElementById(backBtn);
    let right = document.getElementById(rightBtn);
    if (currIndex > 0) left.style.display = "block";
    else left.style.display = "none";
    if (currIndex  == MaxIndex-1) right.style.display = "none";
    else right.style.display = "block";

}




//Book Full INformation Page Display
function ShowFullInfo(book) {
    let bookObj = JSON.parse(decodeURIComponent(book));
    document.getElementById("home-books").style.display = "none";
    console.log(bookObj);
}