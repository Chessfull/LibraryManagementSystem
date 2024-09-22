console.log("External script loaded!");

function confirmDelete(bookId) {
    if (confirm("Are you sure you want to delete this book?")) {
        window.location.href = '/Book/Delete?bookId=' + bookId;
    }
}

function confirmDeleteAuthor(authorId) {
    if (confirm("Are you sure you want to delete this book?")) {
        window.location.href = '/Author/Delete?authorId=' + authorId;
    }
}

function loadBookDetails(button) {

    // Extract data from the clicked button
    var bookId = button.getAttribute('data-book-id')
    var bookTitle = button.getAttribute('data-book-title');
    var bookAuthor = button.getAttribute('data-book-author');
    var publisher = button.getAttribute('data-book-publisher');
    var publicationYear = button.getAttribute('data-book-publishyear');
    var description = button.getAttribute('data-book-description');
    var imageUrl = button.getAttribute('data-book-image');
    
    // Populate the form fields in the modal
    document.getElementById('bookId').value = bookId;
    document.getElementById('bookTitle').value = bookTitle;
    document.getElementById('authorName').value = bookAuthor;
    document.getElementById('publisher').value = publisher;
    document.getElementById('publicationYear').value = publicationYear;
    document.getElementById('description').value = description;
    document.getElementById('imageUrl').value = imageUrl;
}

function loadAuthorDetails(button) {
    var authorId = $(button).attr("author-id");
    var fullName = $(button).attr("author-fullname");
    var birthdate = $(button).attr("author-birthdate");
    var about = $(button).attr("author-about");
    var imageUrl = $(button).attr("author-image-url");

    // Set the form values in the modal
    $('#updateModal #authorId').val(authorId);  // Set AuthorId in the form
    $('#updateModal #authorName').val(fullName);
    $('#updateModal #authorBirthdate').val(birthdate);
    $('#updateModal #authorAbout').val(about);
    $('#updateModal #authorImageUrl').val(imageUrl);
}