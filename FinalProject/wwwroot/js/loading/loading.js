function loadingOut(loading, time) {
    setTimeout(() => loading.out(), time);
}

function verticalTextColor() {
    var loading = new Loading({
        title: 'Xin hãy đợi',
        titleColor: 'rgb(217, 83, 79)',
        discription: 'Loading...',
        discriptionColor: 'rgb(77, 150, 223)',
        animationOriginColor: 'rgb(33, 179, 132)',
        mask: true,
        loadingPadding: '20px 50px',
        defaultApply: true,
    });

    return loading;
}