(
dd if=$1 bs=1 count=9
dd if=/dev/zero bs=1 count=4
dd if=$1 bs=1 skip=9
) | lzma -dc > 'decomp_'$1
