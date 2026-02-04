set tm=%TIME%
set tm=%tm: =_%
git add -A
git commit -m '自動コミット%DATE%%tm%'
git push --progress "origin" main

