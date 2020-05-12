function component() {
    const element = document.createElement('div');

    element.innerHTML = 'The app ran.';

    return element;
}

document.body.appendChild(component());