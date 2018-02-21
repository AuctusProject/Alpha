var baseResponse = function (res) {
    return function (err, result) {
        if (err) next(err);
        else {
            res.status(200).json(result).end();
        };
    }
}

module.exports = baseResponse;

