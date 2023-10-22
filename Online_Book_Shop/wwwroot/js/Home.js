let bookRows = [];
let uniqueId = 0;

GiveBookRow(uniqueId, "Books For Sale", bookList)
//defined at the last

function GiveBookRow(id, heading, BookList,isFiltered) {
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

    if (isFiltered) {
        document.getElementById("filteredBooks").innerHTML = RowContent;
    } else {
        document.getElementById("home-books").innerHTML += RowContent;
    }
    if (Slides.length == 1) document.getElementById(`for-arr${id}`).style.display = "none"
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
                        <input onclick="ShowFullInfo(${book.bookId})" type="button" value="View Info"/>
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
let cart = [];
getCartDataFromDatabase();


let fullInfoOuter = document.getElementById("detailInfo");
function calculatePrice() {
    cart.totalPrice = cart.reduce((acc, c) => acc + (c.book.price * c.quentity), 0)
}
function ShowFullInfo(id) {
    let bookObj = bookList.find(b => b.bookId==id);
    console.log(bookObj, id);
    fullInfoOuter.innerHTML = GiveDetailInfoBook(bookObj);
    document.getElementById('cartFull').style.display = "none";
    fullInfoOuter.style.display = "block";
    document.getElementsByTagName("header")[0].scrollIntoView();

}
function AddToCart(id) {
    let bookObj = bookList.find(b => b.bookId == id);
    storeInDatabase(bookObj);

}
function RemoveFromCart(id) {
    let bookObj = bookList.find(b => b.bookId == id);
    removeFromDatabase(bookObj);
}
async function removeFromDatabase(bookObj) {
    const res = await fetch("/Home/RemoveItemFromCart", {
        body: JSON.stringify({ Book: bookObj, UserId }),
        headers: {
            "Content-Type": "Application/json"
        },
        method: "POST"
    })
    if (res.ok) {
        cart = cart.filter(c => c.book.bookId != bookObj.bookId)
        console.log(bookObj);
        fullInfoOuter.style.display = "none";
        ShowMsg("Item Removed Successfully", 0);
        console.log("cart data after removing", cart);
        RefreshCart();
    } else {
        ShowMsg("Server Error occurred while Removing Item", 1);
    }
}
async function storeInDatabase(bookObj) {
    const res = await fetch("/Home/AddItemToCart", {
        body: JSON.stringify({ Book:bookObj, UserId }),
        headers: {
            "Content-Type":"Application/json"
        },
        method:"POST"
    })
    if (res.ok) {
        cart.push({ book: bookObj, quentity: 1, userid:UserId });
        console.log(bookObj);
        fullInfoOuter.style.display = "none";
        ShowMsg("Item Added Successfully", 0);
    } else {
        ShowMsg("Server Error occurred while Adding Item", 1);
    }
       
}
async function getCartDataFromDatabase() {
    const res = await fetch("/Home/GetCartData", {
        body: JSON.stringify({ ival: UserId }),
        headers: {
            "Content-Type": "Application/json"
        },
        method: "POST"
    })
    if (res.ok) {
        const data = await res.json();
        console.log("cart data", data);
        for (let i = 0; i < data.length; i++) {
            cart.push(data[i]);
        }
    } else {
        ShowMsg("Unable To Retrive The Cart Data",1);
    }
}
async function changeQuentity(id) {
    let bookObjInd = cart.findIndex(b => b.book.bookId == id);
    let bookObj = cart[bookObjInd].book;
    let quentity = document.getElementById(`quentity${id}`).value;
    const res = await fetch("/Home/changeQuentity", {
        body: JSON.stringify({ userId: UserId, book: bookObj, quentity: quentity }),
        headers: {
            "Content-Type": "Application/json"
        },
        method: "POST"
    })
    if (res.ok) {
        cart.totalPrice += (bookObj.price) * (quentity - 1);
        cart[bookObjInd].quentity = quentity;
        RefreshCart();
    } else {
        ShowMsg("Error While Changing Quentity",1);
        document.getElementById(`quentity${id}`).value = quentity;
    }
}

function GiveDetailInfoBook(book) {
    return `
        <div id="home-full" class="home-full">
    <div class="full-img">
        <img src="/Images//BookImages/${book.imageFileName}"/>
    </div>
    <div class="full-desc">
        <div class="full-desc-head">
            <h1>${book.name}</h1>
            <p>by ${book.authors.map(a=>a.firstName + a.lastName+' ')}(Author)</p>
        </div>
        <div class="full-desc-rating">
            <div>
                <p>4.6</p>
            </div>
            <div>
                <img src="/Images/icons8-star-filled-96.png" />
                <img src="/Images/icons8-star-filled-96.png" />
                <img src="/Images/icons8-star-filled-96.png" />
                <img src="/Images/icons8-star-filled-96.png" />
                <img src="/Images/icons8-star-half-empty-90.png" />
            </div>
            <div style="margin-left:1rem;">
                <p>11 rating</p>
            </div>
        </div>
        <div class="full-text">
            <p>${book.description}</p>

        </div>
        <div class="full-book-info">
            <div>
                <p>Print length</p>
                <img src="/Images/icons8-pages-100.png"/>
                <p><b>${book.pages} pages</b></p>
            </div>
             <div>
                <p>Language</p>
                <img src="/Images/icons8-earth-52.png"/>
                <p><b>${book.language}</b></p>
            </div>
            <div>
                <p>Publication date</p>
                <img src="/Images/icons8-date-96.png" />
                <p><b>${book.publish.substr(0,10)}</b></p>
            </div>
            <div>
                <p>Dimensions</p>
                <img src="/Images/icons8-dimension-96.png" />
                <p>
                    <b>
                        ${book.width} x ${book.height} inch
                    </b>
                </p>
            </div>
            <div>
                <p>ISBN - 13</p>
                <img src="/Images/icons8-barcode-64.png" />
                <p>
                    <b>
                        ${book.isbn}
                    </b>
                </p>
            </div>
        </div>
    </div>
    <div class="full-cart">
        <div class="price-info">
            <p><b>${book.price.toLocaleString("en-IN", {
                style: "currency",
                currency: "INR"
            })}</b></p>
        </div>
        <button onclick="AddToCart(${book.bookId})" type="button">
            <img src="/Images/icons8-purchase-96.png" />
            <p>Add To Cart</p>
        </button>
    </div>
</div>
    `
}



//show cart information
let cartEle = document.getElementById('cartFull')
//cartEle.style.display = "none";
let cartBtnEle = document.getElementById('cart');
cartBtnEle.addEventListener('click', () => {
    if (cartEle.style.display == 'block') {
        cartEle.style.display = "none";
    } else {
        RefreshCart();
        cartEle.style.display = "block";
    }
    if (document.getElementById('home-full')) {
        document.getElementById("home-full").style.display = "none"
    }
 
});
function RefreshCart() {
    calculatePrice();
    cartEle.innerHTML = getCart();
}
function getCart() {
    return `
    <div class="cart-heading">
            <h2>Shopping Cart</h2>
        </div>
        <div class="cart-outer">
        
        `+ getAllCartItems(cart) +`
    </div>
    ${cart.length > 0 ? `<div class="cart-total">
        <h3> Subtotal(${ cart.length} item) : ${cart.totalPrice.toLocaleString("en-IN", {
            style: "currency",
            currency: "INR"
        })}
        </h3 >
        </div >
        <div class="buy-btn">
            <button onclick="cartPurchase()">Procceed To Buy</button>
        </div>`:""}
    `
}
function getAllCartItems(cart) {
    if (cart.length == 0) return `
      <div class="cart-body">
          <h1>No Cart Item Found</h1>
      </div>
  
    `
    let content = ``;
    for (let i = 0; i < cart.length; i++) {
        content += getCartItem(cart[i].book, cart[i].quentity);
    }
    return content;
}
function getCartItem(book,quentity) {
    return `
        <div class="cart-body">
            <div class="cart-img">
                <img src="/Images/BookImages/${book.imageFileName}" />
            </div>
            <div class="cart-info">
                <div>
                    <h2>${book.name}
                    </h2>
                </div>
                <div>
                    <h4>${book.authors.map(a => a.firstName + a.lastName+'(Author)')}</h4>
                </div>
                <div>
                    <p>${book.price.toLocaleString("en-IN", {
                        style: "currency",
                        currency: "INR"
                    })}</p>
                </div>
                <div>
                    <p style="color:green;">In stock</p>
                </div>
                <div class="buttons">
                    <div class="quentity">
                            <div>
                                <p>Qty.</p>
                                <select onchange="changeQuentity(${book.bookId})" selected=${quentity} id="quentity${book.bookId}" required>
                                    <option ${quentity == 1 ? "selected" : ""} value="1">1</option>
                                    <option ${quentity == 2 ? "selected" : ""} value="2">2</option>
                                    <option ${quentity == 3 ? "selected" : ""} value="3">3</option>
                                    <option ${quentity == 4 ? "selected" : ""} value="4">4</option>
                                    <option ${quentity == 5 ? "selected" : ""} value="5">5</option>
                                </select>
                            </div>
                    </div>
                    <div class="delete-btn">
                        <button onclick="RemoveFromCart(${book.bookId})">Delete</button>
                    </div>
                </div>
            </div>
        </div>
    `
}

async function cartPurchase(){
    const res = await fetch("/Home/cartPurchase", {
        body: JSON.stringify({ ival: UserId }),
        headers: {
            "Content-Type": "Application/json"
        },
        method: "POST"
    });
    if (res.ok) {
        ShowMsg("Order Placed Succesfully", 0);
        cart = [];
        cartEle.style.display = "none";
    } else {
        ShowMsg("Server Error While Placing Order", 1);
    }
    console.log(res);
}



//filtering functionality
let genreEle = document.getElementById("genre-li");
let lanEle = document.getElementById("language-li");
let conEle = document.getElementById("country-li");
let GenreOptions = ["RemoveFilter"]
let languageOptions = ["RemoveFilter"];
let countryOptions = ["RemoveFilter"];

async function fetchData() {
    const res = await fetch("/Admin/GetData", {
        method: "POST",
        headers: {
            "Content-Type": "Application/json",
        },
    })
    if (res.ok) {
        const data = await res.json();
        GenreOptions = [...GenreOptions, ...data.type];
        languageOptions = [...languageOptions, ...data.languages];
        countryOptions = [...countryOptions, ...data.countries];
        //console.log(countryOptions)
    } else {
        showMsg("Failed To Fetch Data From Server", 1);
    }
};
fetchAndAdd();
async function fetchAndAdd() {
    await fetchData();
    let genreUlChild = getOptionsChild(GenreOptions, 'filterGenre',"genreUlChild");
    let languageUlChild = getOptionsChild(languageOptions, 'filterLanguage',"lanUlChild");
    let countryUlChild = getOptionsChild(countryOptions, 'filterCountry',"conUlChild");
    genreEle.appendChild(genreUlChild);
    lanEle.appendChild(languageUlChild);
    conEle.appendChild(countryUlChild);
}
genreEle.addEventListener('click', () => {
    let general = document.getElementById("genreUlChild");
    if (general.style.display == "none") {
        general.style.display = "block"
        document.getElementById("conUlChild").style.display = "none";
        document.getElementById("lanUlChild").style.display = "none";
    } else {
        general.style.display = "none";
    }
})
lanEle.addEventListener('click', () => {
    let general = document.getElementById("lanUlChild");
    if (general.style.display == "none") {
        general.style.display = "block"
        document.getElementById("genreUlChild").style.display = "none";
        document.getElementById("conUlChild").style.display = "none";
    } else {
        general.style.display = "none";
    }
})
conEle.addEventListener('click', () => {
    let general = document.getElementById("conUlChild");
    if (general.style.display == "none") {
        general.style.display = "block"
        document.getElementById("genreUlChild").style.display = "none";
        document.getElementById("lanUlChild").style.display = "none";

    } else {
        general.style.display = "none";
    }
})
let language, country, genre;
function filterCountry(selected) {
    if (selected == "RemoveFilter") {
        country = undefined;
        selected = " jskl"
    } else {
        country = selected;
    }
        document.getElementById("filterBtn").click();
    findElementByContent(document.getElementById('conUlChild'), selected)
}
function filterLanguage(selected) {
    if (selected == "RemoveFilter") {
        language = undefined;
        selected = " jskl"
    } else {
        language = selected;
    }
    document.getElementById("filterBtn").click();
    findElementByContent(document.getElementById('lanUlChild'), selected)
}
function filterGenre(selected) {
    if (selected == "RemoveFilter") {
        genre = undefined;
        selected=" jskl"
    } else {
        genre = selected;
    }
        document.getElementById("filterBtn").click();
    findElementByContent(document.getElementById('genreUlChild'), selected)
}
function findElementByContent(parentElement, textContent) {
    const childElements = parentElement.children;
    for (let i = 0; i < childElements.length; i++) {
        if (childElements[i].textContent.includes(textContent)) {
            childElements[i].style.backgroundImage = "linear-gradient( 135deg, #81FFEF 10%, #F067B4 100%)";
            childElements[i].style.color = "black";
        } else {
            childElements[i].style.background = "#202123";
            childElements[i].style.color = "#E0E0E0";
        }
    }
}
function getOptionsChild(options,methodname,id) {
    let ul = document.createElement("ul");
    ul.id = id;
    for (let i = 0; i < options.length; i++) {
        ul.innerHTML += `
        <li onclick="${methodname}('${options[i]}')">${options[i]} </li>
        `;
    }
    ul.style.flexWrap = "nowrap";
    ul.style.zIndex = 1;
    ul.style.display = "none";
    return ul;
}

document.getElementById("filterBtn").addEventListener('click', () => {
    let searchName = document.getElementById("filterInput").value.toLowerCase();
    console.log(language, country, genre, searchName);
    let fileterMsg = ``;
    let filteredBookList = bookList;
    if (language) {
        filteredBookList = filteredBookList.filter(b => b.language == language);
        fileterMsg += `language : ${language}, `
    }
    if (country) {
        filteredBookList = filteredBookList.filter(b => b.country == country);
        fileterMsg += `country : ${country}, `
    }
    if (genre) {
        filteredBookList = filteredBookList.filter(b => b.type == genre);
        fileterMsg += `genre : ${genre}, `
    }
    if (searchName) {
        filteredBookList = filteredBookList.filter(b => b.name.toLowerCase().includes(searchName));
        fileterMsg += `Name : ${searchName}, `
    }
    if (filteredBookList.length > 0) {
        document.getElementById("filteredBooks").style.display = "block";
        GiveBookRow(uniqueId, "", filteredBookList, true)
    } else {
        ShowMsg(`No Book Found With the ${fileterMsg}`, 1)
        document.getElementById("filteredBooks").style.display = "none";
    }
})
document.getElementById("filterInput").addEventListener("input", () => {
    console.log("helo")
    document.getElementById("filterBtn").click();
})
document.getElementById("clearfilters").addEventListener("click", () => {
    language = undefined;
    country = undefined;
    genre = undefined;
    document.getElementById("filterInput").value = '';
    document.getElementById("filteredBooks").style.display = "none";
})